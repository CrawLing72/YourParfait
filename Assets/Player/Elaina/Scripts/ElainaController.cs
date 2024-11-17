using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
#endif

public class ElainaController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    [SerializeField]
    protected GameObject butterflyPrefb;

    [SerializeField]
    protected GameObject skillEEffect;

    [SerializeField]
    protected GameObject skillREffect;


    protected override void Start()
    {
        base.Start(); 

    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        CastWSkill();
        CastRSkill();
    }

    private void CastWSkill()
    {
        if (wIsOn)
        {


            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("W is On");

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 dir = (mousePos - myPos).normalized;

                GameObject wSkillPrefeb = Instantiate(butterflyPrefb);
                NonTargetSkill nonTargetSkill = wSkillPrefeb.GetComponent<NonTargetSkill>();
                nonTargetSkill.SetSkillDamage(10.0f); // ?? ??

                wSkillPrefeb.transform.position = myPos + dir * 1.0f;
                nonTargetSkill.SetDirection(dir);

                isWAble = false;
                currentWTime = stat.GetWTime();

                wIsOn = false;
            }
        }
    }

    private void CastRSkill()
    {
        if (qIsOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                new WaitForSeconds(1.0f);
                GameObject attack = Instantiate(skillREffect);
                NonTargetThrow nonTargetThrow = attack.GetComponent<NonTargetThrow>();
                nonTargetThrow.SetSkillDamage(10.0f); // 수정 필요

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 dir = (mousePos - myPos).normalized;
                float angle = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;

                attack.transform.rotation = Quaternion.Euler(0, 0, angle);
                attack.transform.position = gameObject.transform.position;

                isQAble = false;
                currentQTime = stat.GetQTime();

                qIsOn = false;
            }
        }
    }

    protected override void Attack(GameObject Target)
    {
        GameObject attack = Instantiate(attackPrefeb);
        RangeAttack attackFunc = attack.GetComponent<RangeAttack>();

        attack.transform.position = gameObject.transform.position;
        attackFunc.GetTarget(Target, stat.GetAd()); 
    }

    protected override void InputActionW()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
           
            if (isWAble)
            {
                
                wIsOn = true;
            }
            else
            {
                wIsOn = false;
            }
        }

    }

    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isEAble)
            {
                GameObject attack = Instantiate(skillEEffect);
                BuffSkill eSkill = attack.GetComponent<BuffSkill>();
                attack.transform.position = gameObject.transform.position;

                currentETime = stat.GetETime();

                isEAble = false;
            }
        }

    }

    protected override void InputActionQ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isQAble)
            {
                qIsOn = true;
            }
            else
            {
                qIsOn = false;
            }
        }
    
    }
}
