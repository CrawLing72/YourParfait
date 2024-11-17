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


    float adBuffTemp;
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


    protected bool shouldFire = false;
    private Transform Char;
    private SkeletonAnimation skeletonAnimation;

    bool isSilent = false;

    protected GameObject QSkillRangePrefeb;
    protected GameObject ESkillRangePrefeb;
    protected GameObject WSkillRangePrefeb;

    [SerializeField]
    protected float QSkillRange;

    [SerializeField]
    protected float WSkillRange;

    [SerializeField]
    protected float ESkillRange;

    [SerializeField]
    protected GameObject skillEPreFeb;

    [SerializeField]
    protected GameObject skillQPreFeb;

    [SerializeField]
    protected GameObject skillWPreFeb;



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

        QSkillRangePrefeb = transform.Find("QRange").gameObject;

        if (QSkillRangePrefeb != null)
        {
            QSkillRangePrefeb.SetActive(qIsOn);
        }


        ESkillRangePrefeb = transform.Find("ERange").gameObject;
        if (ESkillRangePrefeb != null)
        {
            ESkillRangePrefeb.SetActive(eIsOn);
        }

        WSkillRangePrefeb = transform.Find("WRange").gameObject;
        if (WSkillRangePrefeb != null)
        {
            WSkillRangePrefeb.SetActive(eIsOn);
        }
    }

    public override void FixedUpdateNetwork()
    {
        MouseRightClick();
        settingAnimation();

        Attack2();
        MouseRightClick();

        if(!isSilent)
        {
            InputActionW();
            InputActionE();
            InputActionQ();
        }

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

    protected virtual void InputActionW() 
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.W))
        {

            if (wIsOn)
            {
                wIsOn = false;

                WSkillRangePrefeb.SetActive(wIsOn);
            }
            else
            {
                if (isWAble)
                {
                    wIsOn = true;

                    eIsOn = false;
                    qIsOn = false;

                    QSkillRangePrefeb.SetActive(qIsOn);
                    WSkillRangePrefeb.SetActive(wIsOn);
                    ESkillRangePrefeb.SetActive(eIsOn);

                }
            }
        }
    }
    protected virtual void InputActionE() 
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.E))
        {

            if (eIsOn)
            {
                eIsOn = false;

                ESkillRangePrefeb.SetActive(eIsOn);
            }
            else
            {
                if (isEAble)
                {
                    eIsOn = true;

                    wIsOn = false;
                    qIsOn = false;

                    QSkillRangePrefeb.SetActive(qIsOn);
                    WSkillRangePrefeb.SetActive(wIsOn);
                    ESkillRangePrefeb.SetActive(eIsOn);

                }
            }

        }

    }


    protected virtual void InputActionQ() 
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
        {
            if (qIsOn)
            {
                qIsOn = false;

                QSkillRangePrefeb.SetActive(qIsOn);
            }
            else
            {
                if (isQAble)
                {
                    qIsOn = true;

                    wIsOn = false;
                    eIsOn = false;

                    QSkillRangePrefeb.SetActive(qIsOn);
                    WSkillRangePrefeb.SetActive(wIsOn);
                    ESkillRangePrefeb.SetActive(eIsOn);

                }
            }
        }
    }
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

                    GameObject projectile = Instantiate(BasicAttack, objectPos + dir*1.7f, Quaternion.identity); // Need Change 
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

    public void GetSilent(float time)
    {
        isSilent = true;

        qIsOn = false; wIsOn = false; eIsOn = false;
        

        if (QSkillRangePrefeb != null) 
        {
            QSkillRangePrefeb.SetActive(false);
        }
        if (WSkillRangePrefeb != null)
        {
            WSkillRangePrefeb.SetActive(false);
        }
        if (ESkillRangePrefeb != null)
        {
            ESkillRangePrefeb.SetActive(false);
        }

        Invoke("OffSilent", time);
    }

    

    void OffSilent()
    {
        isSilent = false;
    }

    public void GetSlow(float value, float time)
    {

    }

    void OffSlow()
    {

    }

    void GetAdBuff(float value, float time)
    {
        stat.SetAd(stat.GetAd() + value);
        adBuffTemp = value;
        Invoke("OffAdBuff", time);
    }

    void OffAdBuff()
    {
        stat.SetAd(stat.GetAd());
    }

    void OnQ()
    {
        isQAble = true;
    }

    void OnW()
    {
        isWAble = true;
    }

    void OnE()
    {
        isEAble= true;
    }

    void OnAttack()
    {
        isAttackAble = true;
    }
}