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
    bool isFactoryBuff = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            if (isFactoryBuff)
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

}
