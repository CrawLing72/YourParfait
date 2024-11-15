using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetThrow : MonoBehaviour
{

    protected float damage;

    protected bool silent = false;
    protected float silentTime;

    protected bool slow = false;
    protected float slowTime;
    protected float slowValue;

    protected bool bisbondage;

    bool bTeam;

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

    public void GetTeam(bool team)
    {
        bTeam = team;
    }
}
