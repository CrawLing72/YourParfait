using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetSkill : NonTargetThrow, IAttack
{
    Stat playerStat;

    [SerializeField]
    float speed;

    [SerializeField]
    float time;

    /*
    bool silent = false;
    float silentTime;

    bool slow = false;
    float slowTime;
    float slowValue;
    */


//    private float damage;


    Vector2 dir;

    private void Start()
    {
        Destroy(gameObject, time);
    }

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log(collision.gameObject.name);

            IAttack target = collision.gameObject.GetComponent<IAttack>();
            if (target != null)
            {
                target.GetDamage(damage);

                if (silent)
                {
                    target.GetSilent(silentTime);
                }

                if (slow)
                {
                    target.GetSlow(slowValue, slowTime);
                }

                Destroy(gameObject);
            }
        }
    }

    public void SetStat(Stat stat)
    {
        playerStat = stat;
    }


    public void SetDirection(Vector2 diretion)
    {
        dir = diretion;
    }

}
