using Fusion;
using Fusion.Sockets;
using Spine.Unity;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BasicController : NetworkBehaviour, IAttack
{

    [Header("Skill")]
    [SerializeField]
    protected GameObject BasicAttack;

    protected Rigidbody2D rb;
    protected Camera cam;
    protected Stat stat;

    protected bool isAttackAble = true, isQAble = true, isWAble = true, isEAble = true;
    protected bool qIsOn = false, wIsOn = false, eIsOn = false;
    protected float currentAttackTime, currentQTime, currentWTime, currentETime;


    float adBuffTemp;
    protected Vector3 Scale;

    protected Vector3 CurrenPosition = new Vector3(0, 0, 0);


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
    protected SkeletonAnimation skeletonAnimation;
    protected string AnimName = "idle";

    bool isSilent = false;

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

    float ADBuffTemp;
    float TempSpeed;

    [Networked]
    public bool isRedTeam { get; set;}

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Stat>();
        cam = Camera.main;
        Char = gameObject.transform.GetChild(0);
        skeletonAnimation = Char.GetComponent<SkeletonAnimation>();
        CurrenPosition = gameObject.transform.position;

        Scale = Char.localScale;
    }

    protected virtual void Start()
    {
        stat.SetSpeed(50.0f);
        CurrenPosition = gameObject.transform.position;


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

        GameUIManager.instance.SetETimer(currentETime);
        GameUIManager.instance.SetQTimer(currentQTime);
        GameUIManager.instance.SetWTimer(currentWTime);

        CurrenPosition = gameObject.transform.position;

        ApplySkillEffect();

    }

    public override void Spawned()
    {
        base.Spawned();
        isRedTeam = GameManager.instance.isRedTeam;
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

                skillWPreFeb.SetActive(wIsOn);
            }
            else
            {
                if (isWAble)
                {
                    wIsOn = true;

                    eIsOn = false;
                    qIsOn = false;

                    skillQPreFeb.SetActive(qIsOn);
                    skillWPreFeb.SetActive(wIsOn);
                    skillEPreFeb.SetActive(eIsOn);

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

                skillEPreFeb.SetActive(eIsOn);
            }
            else
            {
                if (isEAble)
                {
                    eIsOn = true;

                    wIsOn = false;
                    qIsOn = false;

                    skillQPreFeb.SetActive(qIsOn);
                    skillWPreFeb.SetActive(wIsOn);
                    skillEPreFeb.SetActive(eIsOn);

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

                skillQPreFeb.SetActive(qIsOn);
            }
            else
            {
                if (isQAble)
                {
                    qIsOn = true;

                    wIsOn = false;
                    eIsOn = false;

                    skillQPreFeb.SetActive(qIsOn);
                    skillWPreFeb.SetActive(wIsOn);
                    skillEPreFeb.SetActive(eIsOn);

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

                    NetworkObject projectile = NetworkManager.Instance.runner.Spawn(BasicAttack, objectPos + dir*1.7f, Quaternion.identity); // Need Change 
                    projectile.gameObject.GetComponent<NonTargetSkill>().SetDirection(dir);
                    projectile.gameObject.GetComponent<NonTargetSkill>().SetSkillDamage(stat.GetAd());

                    isAttackAble = false;
                    currentAttackTime = stat.GetAttackTime();
                }
           }
    }
    public void GetDamage(float Damage)
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
        if(AnimName != "idle")
        {
            CurrentAnimation = AnimName;
        }
        else
        {
            if(rb.velocity.magnitude > 0)
            {
                CurrentAnimation = "walking";
            }
            else
            {
                CurrentAnimation = "idle";
            }
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
        

        if (skillQPreFeb != null) 
        {
            skillQPreFeb.SetActive(false);
        }
        if (skillWPreFeb != null)
        {
            skillWPreFeb.SetActive(false);
        }
        if (skillEPreFeb != null)
        {
            skillEPreFeb.SetActive(false);
        }

        Invoke("OffSilent", time);
    }

    

    void OffSilent()
    {
        isSilent = false;
    }

    void GetSlow(float value, float time)
    {

    }

    void OffSlow()
    {

    }

    void GetStop()
    {

    }

    void Getbondage(float time)
    {

    }

    protected void GetAdBuff(float value, float time)
    {
        stat.SetAd(stat.GetAd() + value);
        adBuffTemp = value;
        Invoke("OffAdBuff", time);
    }

    void OffAdBuff()
    {
        stat.SetAd(stat.GetAd() - adBuffTemp);
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

    protected void SettingAnimationIdle()
    {
        AnimName = "idle";
    }

    virtual protected void ApplySkillEffect()
    {

    }
}