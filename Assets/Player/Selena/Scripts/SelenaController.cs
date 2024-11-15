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


    float CurrentShild = 0.0f;
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

        //        skillWPreFeb = transform.GetChild(4).gameObject;

        IsShild = false;
        skillWPreFeb = transform.Find("Shild").gameObject;

        if(skillWPreFeb != null)
        {
            skillWPreFeb.SetActive(IsShild);
        }
        

        
    }


    protected override void InputActionW() 
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            IsShild = true;
            CurrentShild = 200.0f;

            skillWPreFeb.SetActive(IsShild);

            Invoke("OffShild", 5);
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

                    skill.SetSkillDamage(10.0f); // need Change
                    attack.transform.position = mousePos;
                    isQAble = false;
                    currentQTime = stat.GetQTime();
                }

                qIsOn = false;

                QSkillRangePrefeb.SetActive(qIsOn);

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
                    skill.SetSkillDamage(10.0f); // need Change
                    attack.transform.position = mousePos;



                    isEAble = false;
                    currentETime = stat.GetETime();
                }

                eIsOn = false;

                ESkillRangePrefeb.SetActive(eIsOn);

            }
        }    
        else
        {
            base.Attack2();
        }
    }

    protected override void GetDamage(float Damage, MonoBehaviour DamageCauser)
    {
        if(IsShild)
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
        }
    }

    void OffShild()
    {
        IsShild = false;
        skillWPreFeb.SetActive(IsShild);
    }

}
