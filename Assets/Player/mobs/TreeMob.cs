using Fusion;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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

    public AK.Wwise.Event idleEvent;
    public AK.Wwise.Event attackEvent;
    public GameObject SoundObject;

    public float huhTime = 5f;

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
            timer += Time.deltaTime;
            skeletonAnimation.AnimationName = "attack 2";
            if(timer >= attackTime)
            {
                GameObject obj = Instantiate(SoundObject, transform.position, Quaternion.identity);
                SoundController sound = obj.GetComponent<SoundController>();
                sound.PlaySound(attackEvent);
                timer = 0f;
            }
        }
        else
        {
           timer += Time.deltaTime;
            skeletonAnimation.AnimationName = "idle";
            if (timer >= huhTime)
            {
                GameObject obj = Instantiate(SoundObject, transform.position, Quaternion.identity);
                SoundController sound = obj.GetComponent<SoundController>();
                sound.PlaySound(idleEvent);
                timer = 0f;
            }
        }

        healthBar.value = health / maxHealth;
    }

    public override void FixedUpdateNetwork()
    {
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();

        if(timer >= attackTime)
        {
            if (target != null)
            {
                NetworkObject targetObj = target.gameObject.GetComponent<NetworkObject>();
                IAttack target_I = targetObj?.GetComponent<IAttack>();

                if (targetObj != null && !targetObj.HasStateAuthority) // 본인이 마스터 클라이언트가 아닌 경우
                {
                    string tag = targetObj.gameObject.tag;
                    switch (tag)
                    {
                        case "Player":
                            gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, false, 0, false, 0, 0);
                            break;
                        default:
                            break;

                    }

                }
                else if (targetObj != null) // -> 본인이 마스터 클라이언트인 경우, 혹은 해당 Obj에 StateAuthority를 가지고 있는 경우
                {
                    string tag = targetObj.gameObject.tag;
                    switch (tag)
                    {
                        case "Player":
                            target_I.GetDamage(damage);
                            break;
                        default:
                            break;

                    }
                }

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
