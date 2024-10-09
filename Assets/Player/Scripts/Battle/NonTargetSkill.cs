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
                target.GetDamage(10.0f);
                Destroy(gameObject);
            }
        }
    }

    public void GetStat(Stat stat)
    {
        playerStat = stat;
    }

    public void GetDirection(Vector2 diretion)
    {
        dir = diretion;
    }
}
