using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : MonoBehaviour
{

    GameObject player;


    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            IAttack target = collision.gameObject.GetComponent<IAttack>();


            if (target != null)
            {
            
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider != null)
        {

        }
    }

    public void SetTarget(GameObject target)
    {
        player = target;
    }


}
