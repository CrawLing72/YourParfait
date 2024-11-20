using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphinaController : BasicController, IAttack
{

    protected override void Start()
    {
        base.Start();
        skillEPreFeb.SetActive(false);

    }
    protected override void InputActionW() 
    {
        if (isWAble)
        {
            float currentHp = stat.GetCurrentHp();
            float healAmount = 150.0f;
            stat.SetCurrentHp(currentHp + healAmount);
            GetAdBuff(50.0f, 3);

            isWAble = false;
            currentWTime = stat.GetWTime();
        }
    }
    protected override void InputActionE() 
    {
        if(isEAble)
        {
            GameObject attack = Instantiate(skillEPreFeb);
            skillEPreFeb.SetActive(true);
            Invoke("OffE", 3);

        }
    }

    void OffE()
    {
        skillEPreFeb.SetActive(false);
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
                attack.transform.rotation = Quaternion.AngleAxis(angel, new Vector3(0, 0, 1));
                attack.transform.position = myPos + dir * 2.0f;
                


                isQAble = false;
                currentQTime = stat.GetQTime();

                qIsOn = false;
                skillQPreFeb.SetActive(qIsOn);

            }
        }
        else
        {
            base.Attack2();
        }
    }

    void IAttack.GetDamage(float Damage)
    {
        GameState Instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        Debug.LogError("Selena Got Damage!");
        float CurrentHp = stat.GetCurrentHp();
        Debug.LogError(CurrentHp);
        stat.SetCurrentHp(CurrentHp - Damage);
        Instance.RPC_SetHP(stat.clientIndex, stat.GetCurrentHp(), stat.GetMaxHp());
        Object.RequestStateAuthority(); // State Authority È¸º¹
    }
}
