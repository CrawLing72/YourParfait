using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelenaController : BasicController
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


    [Header("Skill")]
    [SerializeField]
    protected GameObject skillEPreFeb;

    [SerializeField]
    protected GameObject skillQPreFeb;

    [SerializeField]
    protected GameObject skillWPreFeb;

    [SerializeField]
    float QSkillRange;

    bool IsShild = false;

    
    protected override void Start()
    {
        base.Start();
        stat.SetMaxHp(maxHp);
        stat.SetMaxMp(maxMp);
        stat.SetCurrentHp(currentHp);
        stat.SetCurrentMp(currentMp);
        stat.SetSpeed(speed);
        stat.SetAttackRange(attackRang);
        stat.SetAttackTime(attackTime);
        stat.SetTWime(wTime);
        stat.SetTEime(eTime);
        stat.SetQTime(qTime);
        stat.SetAd(ad);
    }

    private void Update()
    {
        /*
        if ( (!qIsOn) && (!wIsOn) && (!eIsOn))
        {
            Debug.Log("Should Fire");
            if (Input.GetMouseButtonDown(0))
            {
                if (isAttackAble)
                {
                    lastMouseClickPosition = Input.mousePosition;
                    lastMouseClickPosition.z = Mathf.Abs(cam.transform.position.z);
                    lastMouseClickPosition = cam.ScreenToWorldPoint(lastMouseClickPosition);
                    shouldFire = true; // 발사 플래그 설정
                }
                else
                {
                    shouldFire = false;
                }
            }

        }
        */
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        


        /*
        if (shouldFire)
        {
            MouseLeftClick(lastMouseClickPosition);
        }
        */
        
    }

    protected override void InputActionW() 
    {

    }

    protected override void InputActionE() 
    {

    }

    protected override void InputActionQ() 
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            if (isQAble)
            {
                qIsOn = true;

                wIsOn = false;
                eIsOn = false;

                Debug.Log("QISOn");
            }
            else
            {
                qIsOn = false;

                Debug.Log("QISOFF");
            }
        }
    }

    protected override void Attack2()
    {

        if (qIsOn)
        {
            Debug.Log("QIsable");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("CastQ");

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 disTance = mousePos - myPos;

                if (disTance.magnitude <= QSkillRange)
                {
                    GameObject attack = Instantiate(skillQPreFeb);
                    NonTargetThrow skill = attack.GetComponent<NonTargetThrow>();

                    skill.SetSkillDamage(10.0f); // need Change
                    attack.transform.position = mousePos;
                    isQAble = false;
                    currentQTime = stat.GetQTime();
                }

                qIsOn = false;
            }
        }
        else
        {
            base.Attack2();
        }
    }

    public override void GetDamage(float Damage, MonoBehaviour DamageCauser)
    {
        if(IsShild)
        {

        }
        else
        {
            base.GetDamage(Damage, DamageCauser);
        }
    }

}
