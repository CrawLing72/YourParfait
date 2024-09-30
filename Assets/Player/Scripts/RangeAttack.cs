using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    GameObject target; 
    protected Vector2 vel;
    protected Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {

        Vector2 myLocation = gameObject.transform.position;
        Vector2 targetLocation = target.transform.position;

        vel = (targetLocation - myLocation).normalized;

        rb.velocity = vel * 3000.0f * Time.deltaTime;

        if (Mathf.Abs((myLocation - targetLocation).magnitude) < 1.0f) 
        {
            Destroy(gameObject);
        }

        //Debug.Log(vel.ToString());

    }

    public void GetTarget(GameObject Target) 
    {
        target = Target;
    }
}
