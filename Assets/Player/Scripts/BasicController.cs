using System.Collections;
using System.Collections.Generic;
using Fusion;
using Spine.Unity;
using UnityEngine;

public class BasicController : NetworkBehaviour, IAttack
{

    [Header("Skill")]
    [SerializeField]
    GameObject BasicAttack;

    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected bool isAttackAble = true, isWAble = true, isEAble = true, isRAble = true;
    protected float currentAttackTime, currentWTime, currentRTime, currentETiem;

    protected Vector2 mouseClickPos;

    private bool onDirection = true;
    protected bool shouldFire = false;
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

    protected void MouseLeftClick(Vector3 mouseClickPos) // Basic Attack
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseClickPos, Vector2.zero, Mathf.Infinity);

        if (hit.collider != null)
        {
            Debug.Log($"Hit Object: {hit.collider.name}");

            Vector2 objectPos = gameObject.transform.position;
            Vector2 distance = ((Vector2)mouseClickPos - objectPos);

            Debug.Log($"Distance: {distance.magnitude}, Required Range: {stat.GetAttackRange()}");

            if (distance.magnitude < stat.GetAttackRange() && isAttackAble)
            {
                Vector3 projectilePos = gameObject.transform.position;
                projectilePos.x += (distance.x < 0 ? -2 : 1); // Object Axis Imbalance로 인한 보정
                projectilePos.y -= 1;

                GameObject projectile = Instantiate(BasicAttack, projectilePos, Quaternion.identity);
                projectile.GetComponent<NonTargetSkill>().GetDirection(distance.normalized);

                Debug.Log("Projectile Launched");

                isAttackAble = false;
                currentAttackTime = stat.GetAttackTime();
                shouldFire = false; // 발사 후 플래그 초기화
            }
        }
        else
        {
            Debug.Log("No Object Hit Detected");
            shouldFire = false; // 발사 실패 시 플래그 초기화
        }
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
