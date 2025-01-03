using Fusion;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelenaController : BasicController, IAttack
{

    [Header("Status")]
    public float maxHp;
    public float maxMp;
    public float currentHp;
    public float currentMp;
    public float speed;
    public float attackRang;
    public float attackTime;
    public float wTime;
    public float eTime;
    public float qTime;
    public float ad;
    private Vector3 lastMouseClickPosition;


    float CurrentShild = 0.0f;
    bool IsShild = false;

    
    protected override void Start()
    {
        base.Start();

        //        skillWPreFeb = transform.GetChild(4).gameObject;

        IsShild = false;
        skillWPreFeb = transform.Find("Shild").gameObject;

        if(skillWPreFeb != null)
        {
            skillWPreFeb.SetActive(IsShild);
        }

        skeletonAnimation.name = "idle";
        
    }


    protected override void InputActionW() // Selena : Shield on
    {
        if (Input.GetKeyDown(KeyCode.W) && isWAble)
        {
            IsShild = true;
            CurrentShild = 200.0f;

            skillWPreFeb.SetActive(IsShild);

            Invoke("OffShild", 5);
            currentWTime = stat.GetWTime();

            if(Object.HasStateAuthority) SpawnSoundPrefab("W");
            else Rpc_Sound("W");
        }
    }

    protected override void InputActionE() // Selena : 마석 던져서 대폭발 시키기
    {
        if (Input.GetKeyDown(KeyCode.E) && isEAble)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = CurrenPosition;
            Vector2 disTance = mousePos - myPos;

            if (disTance.magnitude < ESkillRange)
            {
                NetworkObject attack = NetworkManager.Instance.runner.Spawn(skillEPreFeb, CurrenPosition, Quaternion.identity);
                NonTargetSkill skill = attack.GetComponent<NonTargetSkill>();
                if (isRedTeam) attack.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("RedTeam");
                else attack.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("BlueTeam");


                Animator animator = attack.gameObject.GetComponent<Animator>();

                animator.SetBool("isE", true);
                skill.SetSkillDamage(400.0f); // need Change
                skill.SetTime(1.7f);
                attack.transform.position = mousePos;
                isEAble = false;
                currentETime = stat.GetETime();
                AnimName = "AtribinJoint";
                Invoke("SettingAnimationIdle", 1.33f);

                if(Object.HasStateAuthority) SpawnSoundPrefab("E");
                else Rpc_Sound("E");

            }
            else
            {
                return; //Early Termination
            }

        }
    }

    protected override void InputActionQ() // Selena : 마석 던지기
    {
        if (Input.GetKeyDown(KeyCode.Q) && isQAble)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = CurrenPosition;
            Vector2 disTance = mousePos - myPos;

            if (disTance.magnitude < QSkillRange)
            {
                NetworkObject attack = NetworkManager.Instance.runner.Spawn(skillQPreFeb, CurrenPosition, Quaternion.identity);
                if (isRedTeam) attack.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("RedTeam");
                else attack.gameObject.GetComponent<NonTargetSkill>().hitCollider.excludeLayers = LayerMask.GetMask("BlueTeam");

                NonTargetSkill skill = attack.GetComponent<NonTargetSkill>();

                skill.SetSkillDamage(200.0f); // need Change
                skill.SetTime(1.2f);
                attack.transform.position = mousePos;
                isEAble = false;
                currentETime = stat.GetQTime();
                AnimName = "AtribinJoint";
                Invoke("SettingAnimationIdle", 1.33f);

                if(Object.HasStateAuthority) SpawnSoundPrefab("Q");
                else Rpc_Sound("Q");

            }
            else
            {
                return; //Early Termination
            }

        }
    }

    protected override void Attack2()
    {

        if (qIsOn)
        {
            if (Input.GetMouseButtonDown(0))
            {

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 disTance = mousePos - myPos;

                if (disTance.magnitude <= QSkillRange)
                {
                    GameObject attack = Instantiate(skillQPreFeb);
                    NonTargetThrow skill = attack.GetComponent<NonTargetThrow>();

                    skill.SetSkillDamage(60.0f); // need Change
                    attack.transform.position = mousePos;
                    isQAble = false;
                    currentQTime = stat.GetQTime();
                }

                qIsOn = false;

                skillQPreFeb.SetActive(qIsOn);

                SpawnSoundPrefab("BAK");

            }
        }
        else if(eIsOn)
        {
            if (Input.GetMouseButtonDown(0))
            {

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 disTance = mousePos - myPos;

                if (disTance.magnitude <= ESkillRange)
                {
                    GameObject attack = Instantiate(skillEPreFeb);
                    NonTargetThrow skill = attack.GetComponent<NonTargetThrow>();


                    skill.SetSilent(3);
                    skill.SetSkillDamage(60.0f); // need Change
                    attack.transform.position = mousePos;



                    isEAble = false;
                    currentETime = stat.GetETime();
                }

                eIsOn = false;

                skillEPreFeb.SetActive(eIsOn);

            }
        }    
        else
        {
            base.Attack2();
        }
    }

    public new void GetDamage(float Damage)
    {
        GameState Instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (IsShild)
        {
            CurrentShild = CurrentShild - Damage;
            if(CurrentShild <= 0)
            {
                IsShild = false;
                skillWPreFeb.SetActive(IsShild);

                float CurrentHp = stat.GetCurrentHp();
                stat.SetCurrentHp(CurrentHp - Damage);
            }
        }
        else
        {
            float CurrentHp = stat.GetCurrentHp();
            stat.SetCurrentHp(CurrentHp - Damage);
            Instance.RPC_SetHP(stat.clientIndex, stat.GetCurrentHp(), stat.GetMaxHp());
        }
    }

    void OffShild()
    {
        IsShild = false;
        skillWPreFeb.SetActive(IsShild);
    }

}
