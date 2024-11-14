using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetThrow : MonoBehaviour
{

    private float damage;

    bool silent = false;
    float silentTime;

    bool slow = false;
    float slowTime;
    float slowValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            Debug.Log("Collison on");

            IAttack target = collision.gameObject.GetComponent<IAttack>();

            if (target != null)
            {
                target.GetDamage(damage);

                if(silent)
                {
                    target.GetSilent(silentTime);
                }

                if (slow)
                {
                    target.GetSlow(slowValue, slowTime);
                }
            }
        }
    }

    public void SetSkillDamage(float Damage)
    {
        damage = Damage;
    }

    public void SetSilent(float time)
    {
        silentTime = time;
        silent = true;
    }

    public void SetSlow(float time, float value)
    {
        slowTime = time;
        slowValue = value;
        slow = true;
    }
}
