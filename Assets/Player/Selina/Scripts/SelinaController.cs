using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelinaController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    [SerializeField]
    protected GameObject skillWEffect;

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

                GameObject butterfly = Instantiate(skillWEffect);
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



    protected override void Attack(GameObject Target)
    {
        GameObject attack = Instantiate(attackPrefeb);
        RangeAttack attackFunc = attack.GetComponent<RangeAttack>();

        attack.transform.position = gameObject.transform.position;
        attackFunc.GetTarget(Target);
    }

    protected override void InputActionW()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(isWAble)
            {
                currentWTime = stat.GetWTime();


            }

        }

    }

    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isEAble)
            {

            }
        }

    }

    protected override void InputActionQ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isQAble)
            {
               
            }
        }

    }
}
