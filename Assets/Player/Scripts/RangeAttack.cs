using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    GameObject target;
    float Damage;
    protected Vector2 vel;

    public void Update()
    {

        Vector2 myLocation = gameObject.transform.position;
        Vector2 targetLocation = target.transform.position;

        vel = (targetLocation - myLocation).normalized;

        gameObject.transform.Translate(vel * 50.0f * Time.deltaTime);

        if (Mathf.Abs((myLocation - targetLocation).magnitude) < 1.0f) 
        {
            IAttack getAttack = target.GetComponent<IAttack>();
            getAttack.GetDamage(Damage);

            Destroy(gameObject);
        }

        //Debug.Log(vel.ToString());

    }

    public void GetTarget(GameObject Target, float ad)
    {
        target = Target;
        Damage = ad;
    }

}
