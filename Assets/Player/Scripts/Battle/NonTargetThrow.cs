using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetThrow : MonoBehaviour
{

    private float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            Debug.Log("Collison on");

            IAttack target = collision.gameObject.GetComponent<IAttack>();

            if (target != null)
            {
                target.GetDamage(damage);
            }
        }
    }

    public void SetSkillDamage(float Damage)
    {
        damage = Damage;
    }
}
