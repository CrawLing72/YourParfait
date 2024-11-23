using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RainykController : BasicController, IAttack
{
    protected NetworkObject destroyObj;
    protected float w_time = 0;
    protected bool w_time_on = false;
    protected override void Start()
    {
        base.Start();

    }
    protected override void InputActionW() // Current: Seraphina Controller CTRC, CTRV. WARNING!
    {
        if (isWAble)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Buff Effect Not Implemented
                Vector3 interpolation = new Vector3(0f, 2.5f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillWPreFeb, transform.position - interpolation, Quaternion.identity);
                obj.gameObject.transform.SetParent(gameObject.transform, true);
                destroyObj = obj;
                obj.gameObject.SetActive(true);

                // Buff Effect 추가해 놓을 것
                w_time_on = true;

                Invoke("DestroyParticle", 1.0f);
                isWAble = false;
                currentWTime = stat.GetWTime();
            }
        }
    }
    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isEAble)
            {
                float xinterpolation = isLeft ? 5f: -4f;
                Vector3 interpolation = new Vector3(xinterpolation, 1f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillEPreFeb, transform.position - interpolation, Quaternion.identity);
                FrontSkill objSkill = obj.gameObject.GetComponent<FrontSkill>();
                obj.gameObject.transform.SetParent(gameObject.transform, true);


                destroyObj = obj;
                obj.gameObject.SetActive(true);

                float current_mp = stat.GetCurrentMp();
                stat.SetCurrentMp(0);
                stat.SetSpeed(stat.GetSpeed() + 10f);
                objSkill.SetDamage(current_mp);
                

                AnimName = "Rux";
                Invoke("SettingAnimationIdle", 2.33f);


                Invoke("OffE", 2.33f);
                Invoke("DestroyParticle", 2.33f);
                isEAble = false;
                currentETime = stat.GetETime();

            }
        }
    }

    protected override void InputActionQ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isQAble)
            {
                Vector3 interpolation = new Vector3(0f, 0f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillQPreFeb, transform.position - interpolation, Quaternion.identity);
                NonTargetSkill objSkill = obj.gameObject.GetComponent<NonTargetSkill>();

                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = Object.transform.position;
                Vector2 disTance = mousePos - myPos;
                Vector2 dir = disTance.normalized;

                objSkill.SetDirection(dir);
                objSkill.SetSkillDamage(200f);

                destroyObj = obj;
                obj.gameObject.SetActive(true);

                AnimName = "ShootFly";
                Invoke("SettingAnimationIdle", 1.66f);


                Invoke("OffQ", 1.66f);
                Invoke("DestroyParticle", 1.66f);
                isQAble = false;
                currentETime = stat.GetQTime();
            }
        }
    }

    protected override void ApplySkillEffect()
    {
        // under : W construction
        if(w_time_on) w_time += NetworkManager.Instance.runner.DeltaTime; 
        if(w_time > 5.0f)
          {
            stat.SetSpeed(stat.GetSpeed() - 10f);
            w_time_on = false;
                w_time = 0;
          }
    }

    void OffE()
    {
        skillEPreFeb.SetActive(false);
    }

    void OffQ()
    {
        skillQPreFeb.SetActive(false);
    }

    void OffW()
    {
        skillWPreFeb.SetActive(false);
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

    public new void GetDamage(float Damage)
    {
        GameState Instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        float CurrentHp = stat.GetCurrentHp();
        stat.SetCurrentHp(CurrentHp - Damage);
        Instance.RPC_SetHP(stat.clientIndex, stat.GetCurrentHp(), stat.GetMaxHp());
        Object.RequestStateAuthority(); // State Authority 회복
    }

    void DestroyParticle()
    {
        NetworkManager.Instance.runner.Despawn(destroyObj);
    }
}
