using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphinaController : BasicController
{
    protected override void InputActionW() 
    {

    }
    protected override void InputActionE() 
    {
        if(isEAble)
        {
            float currentHp = stat.GetCurrentHp();
            float healAmount = 150.0f;
            stat.SetCurrentHp(currentHp + healAmount);

            isEAble = false;
            currentETime = stat.GetETime();
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
                Vector2 dir = disTance.normalized;

                float angel = Vector2.Angle(dir, new Vector2(1, 0));

                GameObject attack = Instantiate(skillQPreFeb);
                NonTargetThrow skill = attack.GetComponent<NonTargetThrow>();

                skill.SetSkillDamage(10.0f); // need Change
                attack.transform.position = myPos + dir * 2.0f;
                attack.transform.rotation = Quaternion.AngleAxis(angel, new Vector3(0, 0, 1));


                isQAble = false;
                currentQTime = stat.GetQTime();

                qIsOn = false;
                QSkillRangePrefeb.SetActive(qIsOn);

            }
        }
        else
        {
            base.Attack2();
        }
    }
}
