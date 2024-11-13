using Fusion;
using Fusion.Sockets;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class BasicController : NetworkBehaviour, IAttack
{

    [Header("Skill")]
    [SerializeField]
    GameObject BasicAttack;

    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected bool isAttackAble = true, isQAble = true, isWAble = true, isEAble = true;
    protected bool qIsOn = false, wIsOn = false, eIsOn = false;
    protected float currentAttackTime, currentQTime, currentWTime, currentETime;

    protected Vector3 Scale;

    // under : network property, do not modify manually!!!! - SHIN
    [Networked, OnChangedRender(nameof(settingNetworkAnim))]
    protected string CurrentAnimation { get; set;}
    [Networked, OnChangedRender(nameof(settingNetworkAnim))]
    protected bool isLeft { get; set; }


    /// <summary>
    /// end of network property
    /// </summary>

    protected Vector2 mouseClickPos;

    private bool onDirection = true;
    protected bool shouldFire = false;
    private Transform Char;
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    protected GameObject CircleRangePrefeb;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Stat>();
        cam = Camera.main;
        Char = gameObject.transform.GetChild(0);
        skeletonAnimation = Char.GetComponent<SkeletonAnimation>();

        Scale = Char.localScale;
    }

    protected virtual void Start()
    {
        stat.SetSpeed(50.0f);
    }

    public override void FixedUpdateNetwork()
    {
        MouseRightClick();
        settingAnimation();

        Attack2();
        InputActionW();
        InputActionE();
        InputActionQ();

        setTimer(ref currentAttackTime, ref isAttackAble);
        setTimer(ref currentQTime, ref isQAble);
        setTimer(ref currentWTime, ref isWAble);
        setTimer(ref currentETime, ref isEAble);

    }

    protected void MouseRightClick()
    {

        Vector2 objectPos = gameObject.transform.position;
        Vector2 distance = (mouseClickPos - objectPos);

        if (UnityEngine.Input.GetMouseButton(1))
        {
            mouseClickPos = UnityEngine.Input.mousePosition;
            mouseClickPos = cam.ScreenToWorldPoint(mouseClickPos);
            rb.velocity = (distance).normalized * stat.GetSpeed() * NetworkManager.Instance.runner.DeltaTime;

        }

        if (Mathf.Abs((distance).magnitude) < 0.5f)
            rb.velocity = Vector2.zero;
        else
        {
            if (distance.x < 0)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }
        }

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
                projectile.GetComponent<NonTargetSkill>().SetDirection(distance.normalized);

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
    protected virtual void InputActionQ() { }
    protected virtual void Attack(GameObject Target) { }

    protected virtual void Attack2()
    {
        
           if (isAttackAble)
           {
               if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    Vector2 mouseLeftPos, objectPos;
                    mouseLeftPos = UnityEngine.Input.mousePosition;
                    mouseLeftPos = cam.ScreenToWorldPoint(mouseLeftPos);
                    objectPos = gameObject.transform.position;
                    Vector2 dir = (mouseLeftPos - objectPos).normalized;

                    GameObject projectile = Instantiate(BasicAttack, objectPos + dir*1.7f, Quaternion.identity);
                    projectile.GetComponent<NonTargetSkill>().SetDirection(dir);

                    isAttackAble = false;
                    currentAttackTime = stat.GetAttackTime();
                }
            }
    }


    public virtual void GetDamage(float Damage, MonoBehaviour DamageCauser)
    {
        float CurrentHp = stat.GetCurrentHp();
        stat.SetCurrentHp(CurrentHp - Damage);
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
            CurrentAnimation = "walking";
        }
        else
        {
            CurrentAnimation = "idle";
        }
        skeletonAnimation.AnimationName = CurrentAnimation;

        if(isLeft)
        {
            Char.localScale = new Vector3(Scale.x, Scale.y, Scale.z);
        }
        else
        {
            Char.localScale = new Vector3(-Scale.x, Scale.y, Scale.z);
        }
    }

    private void settingNetworkAnim()
    {
        skeletonAnimation.AnimationName = CurrentAnimation;
        if (isLeft)
        {
            Char.localScale = new Vector3(Scale.x, Scale.y, Scale.z);
        }
        else
        {
            Char.localScale = new Vector3(-Scale.x, Scale.y, Scale.z);
        }
    }

}