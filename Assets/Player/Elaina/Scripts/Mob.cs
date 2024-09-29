using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IAttack
{

    public void GetDamage(float Damage) 
    {
        Debug.Log("GEtDamageMob");
    }
}
