using Fusion;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TreeMob : NetworkBehaviour, IAttack
{
    [Header("Properties")]
    public int coins;
    public Slider healthBar;
    public float attackTime = 1.4f;
    public SkeletonAnimation skeletonAnimation;
    public float damage = 20f;

    [Networked]
    public float health { get; set; } = 200f;
    public float maxHealth = 200f;

    private float timer = 0f;

    [Networked]
    private bool isPlayerExists { get; set; } = false;
    [Networked]
    private NetworkObject target { get; set; }

    public void FixedUpdate()
    {
        if (isPlayerExists)
        {
            timer += NetworkManager.Instance.runner.DeltaTime;
            skeletonAnimation.AnimationName = "attack 2";
        }
        else
        {
            skeletonAnimation.AnimationName = "idle";
        }

        healthBar.value = health / maxHealth;
    }

    public override void FixedUpdateNetwork()
    {
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();

        if(timer >= attackTime)
        {
            if(target != null && target.HasStateAuthority)
            {
                target.GetComponent<IAttack>().GetDamage(damage);
                timer = 0f;
            }
            else if (target != null && !target.HasStateAuthority)
            {
                NetworkObject targetObj = NetworkManager.Instance.runner.GetPlayerObject(target.StateAuthority);
                targetObj.gameObject.GetComponent<Stat>().SetCurrentHp(targetObj.gameObject.GetComponent<Stat>().GetCurrentHp() - damage);
                Debug.LogError(targetObj.gameObject.GetComponent<Stat>().GetCurrentHp());
                timer = 0f;

            }
        }

        if(health <= 0)
        {
            NetworkManager.Instance.runner.Despawn(Object);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerExists = true;
            target = collision.gameObject.GetComponent<NetworkObject>();
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Object.HasStateAuthority)
        {
            isPlayerExists = true;
            target = collision.gameObject.GetComponent<NetworkObject>();
        }else if (collision.gameObject.CompareTag("Player") && !Object.HasStateAuthority)
        {
            isPlayerExists = true;
            Rpc_SetNetworkObject(collision.gameObject.GetComponent<NetworkObject>());
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<NetworkObject>() == target)
        {
            isPlayerExists = false;
            target = null;
        }
    }

    public void GetDamage(float Damage)
    {
        health -= Damage;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_Damage(float _dm)
    {
        GetDamage(_dm);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_SettingAnimation(string _name)
    {
       skeletonAnimation.AnimationName = _name;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_SetNetworkObject(NetworkObject _target)
    {
        target = _target;
    }
}
