using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : NonTargetThrow
{
    GameObject target;
    protected Vector2 vel;

    public void Update()
    {

        Vector2 myLocation = gameObject.transform.position;
        Vector2 targetLocation = target.transform.position;

        vel = (targetLocation - myLocation).normalized;

        gameObject.transform.Translate(vel * 50.0f * Time.deltaTime);


        //Debug.Log(vel.ToString());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log("Collison on");

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

    public void GetTarget(GameObject Target)
    {
        target = Target;
    }

}
