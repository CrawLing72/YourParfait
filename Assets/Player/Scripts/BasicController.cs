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

        }

        // X축 distance Projectioned Vecotr에 따른 Character 방향 전환
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
