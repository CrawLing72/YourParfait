using Fusion;
using Fusion.Sockets;
using Spine.Unity;
using System;
using TMPro;
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

    [Networked]
    public bool isDead { get; set; } = false;
    public float ReSpawnTime = 30f;
    public float deathAnimTime = 1f;

    public MeshRenderer meshRenderer;

    private Vector3 RedTeamPos = new Vector3(-28.83f, 0.78f, 0);
    private Vector3 BlueTeamPos = new Vector3(29.12f, 0.03f, 0);

    private float SpawnTimer = 0f;
    private float DeadAnimTImer = 0f;

    private bool isDeadAnimEnded = false;

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
        SpawnTimer = ReSpawnTime;


    }

    public override void FixedUpdateNetwork()
    {
        isRedTeam = GameManager.instance.isRedTeam;
        if (isRedTeam) gameObject.layer = 9;
        else gameObject.layer = 10;

        if (!isDead)
        {
            if (stat.GetCurrentHp() <= 0f) // When Dead while playing
            {
                isDead = true;
                
            }

            MouseRightClick();
            settingAnimation();

            Attack2();
            MouseRightClick();

            if (!isSilent)
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
            ApplySkillEffect();
        }
        else
        {
            stat.SetGoodsCount(0);

            GameUIManager.instance.RootObj.SetActive(false);
            GameUIManager.instance.Char_Face.transform.parent.gameObject.SetActive(false); 

            GameUIManager.instance.DeadEffect.SetActive(true);
            GameUIManager.instance.RespawnTimeText.SetActive(true);
            TMP_Text text = GameUIManager.instance.RespawnTimeText.GetComponent<TMP_Text>();
            text.text = "Respawn Time : " + ((int)SpawnTimer).ToString();

            SpawnTimer -= NetworkManager.Instance.runner.DeltaTime;
            DeadAnimTImer += NetworkManager.Instance.runner.DeltaTime;

            if (isRedTeam) gameObject.transform.position = RedTeamPos;
            else gameObject.transform.position = BlueTeamPos;

            if (SpawnTimer < 0f)
            {
                isDead = false;
                SpawnTimer = ReSpawnTime;
                meshRenderer.enabled = true;

                GameUIManager.instance.RootObj.SetActive(true);
                GameUIManager.instance.Char_Face.transform.parent.gameObject.SetActive(true);

                GameUIManager.instance.DeadEffect.SetActive(false);
                GameUIManager.instance.RespawnTimeText.SetActive(false);

                stat.SetCurrentHp(stat.GetMaxHp());
                stat.SetCurrentMp(stat.GetMaxMp());

                SettingAnimationIdle();
            }
            if(DeadAnimTImer > deathAnimTime)
            {
                meshRenderer.enabled = false;
                SettingAnimationIdle();
                isDeadAnimEnded = true;
                DeadAnimTImer = 0f;
            }
            else if (isDeadAnimEnded == false)
            {
                AnimName = "die";
                settingAnimation();
            }
        }

        CurrenPosition = gameObject.transform.position;

    }

    public override void Spawned()
    {
        base.Spawned();
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
                    if(isRedTeam) projectile.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("RedTeam");
                    else projectile.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("BlueTeam");
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