using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BasicController : NetworkBehaviour, IAttack
{
    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected bool isAttackAble, isWAble, isEAble, isRAble;
    protected float currentAttackTime, currentWTime, currentRTime, currentETiem;

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

        setTimer(ref currentAttackTime, ref isAttackAble);
        setTimer(ref currentWTime, ref isWAble);
        setTimer(ref currentETiem, ref isEAble);
        setTimer(ref currentRTime, ref isRAble);

    }

    protected void MouseRightClick()
    {
        Vector2 objectPos = gameObject.transform.position;
        Vector2 distance = (mouseClickPos - objectPos);

        if (Input.GetMouseButton(1))
        {
            mouseClickPos = Input.mousePosition;
            mouseClickPos = cam.ScreenToWorldPoint(mouseClickPos);

            RaycastHit2D hit = Physics2D.Raycast(mouseClickPos, Vector2.zero, 0f);


            if ((hit.collider != null))
            {
                GameObject targetObject = hit.transform.gameObject;

                if (distance.magnitude < stat.GetAttackRange())
                {
                    IAttack targetAttack = targetObject.GetComponent<IAttack>();
                    if ((targetAttack != null) && (isAttackAble == true))
                    {
                        Attack(mouseClickPos);
                        targetAttack.GetDamage(10.0f);

                        isAttackAble = false;
                        currentAttackTime = stat.GetAttackTime();
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

        if (Mathf.Abs((distance).magnitude) < 0.5f)
            rb.velocity = Vector2.zero;
    }

    protected virtual void InputActionW() { }
    protected virtual void InputActionE() { }
    protected virtual void InputActionR() { }
    protected virtual void Attack(Vector2 targetLocation) { }


    public virtual void GetDamage(float Damage)
    {
        Debug.Log("BasicDamage");
    }

    private void setTimer(ref float currentTime, ref bool check)
    {
        if (check == false)
        {
            currentTime -= Runner.DeltaTime;

            if (currentTime < 0)
            {
                check = true;
                currentTime = 0;
            }
        }
    }
}
