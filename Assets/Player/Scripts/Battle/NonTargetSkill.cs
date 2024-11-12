using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetSkill : MonoBehaviour
{
    Stat playerStat;

    [SerializeField]
    float speed;

    [SerializeField]
    float time;


    private float damage;


    Vector2 dir;

    private void Start()
    {
        Destroy(gameObject, time);
    }

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider != null)
        {
            Debug.Log("Collison on");

            IAttack target = collision.gameObject.GetComponent<IAttack>();

            if (target != null)
            {
                target.GetDamage(damage);
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

    public void SetSkillDamage(float Damage)
    {
        damage = Damage;
    }
}
