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

    bool wIsOn = false;
    bool rISOn = false;

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

                GameObject butterfly = Instantiate(butterflyPrefb);
                NonTargetSkill nonTargetSkill = butterfly.GetComponent<NonTargetSkill>();
                nonTargetSkill.SetSkillDamage(10.0f); // ?? ??

                butterfly.transform.position = myPos + dir * 1.0f;
                nonTargetSkill.SetDirection(dir);

                isWAble = false;
                currentWTime = stat.GetWTime();

                wIsOn = false;
            }
        }
    }

    private void CastRSkill()
    {
        if (rISOn)
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

                isRAble = false;
                currentRTime = stat.GetRTime();

                rISOn = false;
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
                eSkill.SetTarget(gameObject);
                attack.transform.position = gameObject.transform.position;

                currentETiem = stat.GetETime();

                isEAble = false;
            }
        }

    }

    protected override void InputActionR()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isRAble)
            {
                rISOn = true;
            }
            else
            {
                rISOn = false;
            }
        }
    
    }
}
