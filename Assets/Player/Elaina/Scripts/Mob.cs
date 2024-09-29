using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IAttack
{

    public virtual void GetDamage(float Damage) 
    {
        Debug.Log("GEtDamageMob");
    }
}
