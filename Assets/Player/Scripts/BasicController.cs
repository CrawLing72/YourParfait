using System.Collections;
using System.Collections.Generic;
using Fusion;
using Spine.Unity;
using UnityEngine;

public class BasicController : NetworkBehaviour, IAttack
{
    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected bool isAttackAble = true, isWAble = true, isEAble = true, isRAble = true;
    protected float currentAttackTime, currentWTime, currentRTime, currentETiem;

    protected Vector2 mouseClickPos;


    private bool inputDelay = true;
    private bool onDirection = true;
    private Transform Char;
    private SkeletonAnimation skeletonAnimation;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Stat>();
        cam = Camera.main;
        Char = gameObject.transform.GetChild(0);
        skeletonAnimation = Char.GetComponent<SkeletonAnimation>();
    }

    protected virtual void Start()
    {

        stat.SetSpeed(50.0f);

    }

    public override void FixedUpdateNetwork()
    {
        MouseRightClick();
        settingAnimation();
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
            rb.velocity = (distance).normalized * stat.GetSpeed() * NetworkManager.Instance.runner.DeltaTime;
            RaycastHit2D hit = Physics2D.Raycast(mouseClickPos, Vector2.zero, 0f);


            if ((hit.collider != null))
            {
                GameObject targetObject = hit.transform.gameObject;

                if (distance.magnitude < stat.GetAttackRange())
                {
                    IAttack targetAttack = targetObject.GetComponent<IAttack>();

                    if ((targetAttack != null) && (isAttackAble == true))
                    {
                        Debug.Log("Attack");
                        Attack(targetObject);

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

        // XÃà distance Projectioned Vecotr¿¡ µû¸¥ Character ¹æÇâ ÀüÈ¯
        Vector3 Scale = Char.localScale;
        if(distance.x < 0)
        {
            Scale.x = (Scale.x < 0 ? -Scale.x : Scale.x);
        }
        else
        {
            Scale.x = (Scale.x > 0 ? -Scale.x : Scale.x);
        }
        Char.localScale = Scale;


        if (Mathf.Abs((distance).magnitude) < 0.5f)
            rb.velocity = Vector2.zero;

    }

    protected virtual void InputActionW() { }
    protected virtual void InputActionE() { }
    protected virtual void InputActionR() { }
    protected virtual void Attack(GameObject Target) { }


    public virtual void GetDamage(float Damage)
    {
        Debug.Log("BasicDamage");
        stat.SetCurrentHp(stat.GetCurrentHp() - Damage);
    }

    private void setTimer(ref float currentTime, ref bool check)
    {
        if (check == false)
        {
            currentTime -= NetworkManager.Instance.runner.DeltaTime;

            if (currentTime <= 0)
            {
                check = true;
                currentTime = 0;
            }
        }
    }

    private void settingAnimation()
    {
        if (rb.velocity.magnitude > 0)
        {
            skeletonAnimation.AnimationName = "walking";
        }
        else
        {
            skeletonAnimation.AnimationName = "idle";
        }
    }
}
