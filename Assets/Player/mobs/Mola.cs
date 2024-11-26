using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mola : NetworkBehaviour, IAttack
{
    [Header("Properties")]
    public int coins;
    public Slider healthBar;

    [Networked]
    public float health { get; set; } = 200f;
    public float maxHealth = 200f;

    public override void FixedUpdateNetwork()
    {
        if(health <= 0)
        {
            NetworkManager.Instance.runner.Despawn(Object);
        }
    }

    public void FixedUpdate()
    {
        healthBar.value = health / maxHealth;
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
