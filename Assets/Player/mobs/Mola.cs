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
    public float huhTime = 5f;
    public GameObject SoundObject;
    public AK.Wwise.Event SoundEvent;

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

    public void Update()
    {
        healthBar.value = health / maxHealth;
        huhTime -= Time.deltaTime;
        if(huhTime <= 0)
        {
            huhTime = 5f;
            GameObject obj = Instantiate(SoundObject, transform.position, Quaternion.identity);
            SoundController sound = obj.GetComponent<SoundController>();
            sound.PlaySound(SoundEvent);
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
