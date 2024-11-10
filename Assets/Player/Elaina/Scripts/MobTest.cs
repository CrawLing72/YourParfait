using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTest : Mob
{
    public override void GetDamage(float Damage)
    {
        Debug.Log("mee toooo, I got " + Damage.ToString());
        Destroy(gameObject);
    }
}
