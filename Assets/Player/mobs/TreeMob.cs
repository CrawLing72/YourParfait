using Fusion;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
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
    private bool isPlayerExists = false;

    private NetworkObject target;
    public override void FixedUpdateNetwork()
    {
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (isPlayerExists)
        {
            timer += NetworkManager.Instance.runner.DeltaTime;
            skeletonAnimation.AnimationName = "attack 2";
        }
        else
        {
            skeletonAnimation.AnimationName = "idle";
        }

        if(timer >= attackTime)
        {
            if(target != null && target.HasStateAuthority)
            {
                target.GetComponent<IAttack>().GetDamage(damage);
                timer = 0f;
            }
            else if (target != null && !target.HasStateAuthority)
            {
                gameState.Rpc_ApplyDamageAndEffects(target.StateAuthority, damage, false, 0f, false, 0f, 0f);
                timer = 0f;

            }
        }

        healthBar.value = health / maxHealth;

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
}
