using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BasicController : NetworkBehaviour, IAttack
{
    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected Vector2 mouseClickPos;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Stat>();

        cam = Camera.main;
    }

    protected virtual void Start()
    {
        stat.SetSpeed(1000.0f);
    }

    public override void FixedUpdateNetwork()
    {
        MouseRightClick();
        InputActionW();
        InputActionE();
        InputActionR();

        
    }

    protected void MouseRightClick()
    {
        Vector2 objectPos = gameObject.transform.position;

        if (Input.GetMouseButton(1))
        {
            mouseClickPos = Input.mousePosition;
            mouseClickPos = cam.ScreenToWorldPoint(mouseClickPos);

            RaycastHit2D hit = Physics2D.Raycast(mouseClickPos, Vector2.zero, 0f);

            Vector2 distance = (mouseClickPos - objectPos);

            if ((hit.collider != null))
            {
                GameObject targetObject = hit.transform.gameObject;

                Debug.Log("Test");

                if (distance.magnitude < stat.GetAttackRange())
                {
                    IAttack targetAttack = targetObject.GetComponent<IAttack>();
                    if (targetAttack != null)
                    {
                        targetAttack.GetDamage(10.0f);
                    }
                    
                    
                }
                else
                {
                    rb.velocity = (distance).normalized * stat.GetSpeed() * Runner.DeltaTime;
                }
                
            }
            else
            {
                rb.velocity = (distance).normalized * stat.GetSpeed() * Runner.DeltaTime;
            }

        }

        if (Mathf.Abs((mouseClickPos - objectPos).magnitude) < 0.5f)
            rb.velocity = Vector2.zero;
    }

    protected virtual void InputActionW() { }
    protected virtual void InputActionE() { }
    protected virtual void InputActionR() { }


    public virtual void GetDamage(float Damage)
    {
        Debug.Log("BasicDamage");
    }

}
