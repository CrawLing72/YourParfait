using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ElainaController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    [SerializeField]
    protected GameObject butterflyPrefb;

    bool wIsOn = false;

    protected override void Start()
    {
        base.Start(); 

        stat.SetMaxHp(1000.0f);
        stat.SetCurrentHp(1000.0f);
        stat.SetMaxMp(1000.0f);
        stat.SetCurrentMp(1000.0f);
        stat.SetAttackRange(10.0f);

        stat.SetAttackTime(1.0f);
        stat.SetTEime(10.0f);
        stat.SetTWime(10.0f);
        stat.SetTRime(10.0f);

        stat.SetAd(10.0f);

    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

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

                butterfly.transform.position = myPos + dir*1.0f;
                nonTargetSkill.GetDirection(dir);

                isWAble = false;
                currentWTime = stat.GetWTime();

                wIsOn = false;
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
            if (isWAble)
            {
            }
        }

    }

    protected override void InputActionR()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isWAble)
            {

            }
        }
    
    }

    public override void GetDamage(float Damage)
    {
        Debug.Log("ElaniaDamage");
    }
}
