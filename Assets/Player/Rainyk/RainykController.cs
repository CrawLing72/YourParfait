using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RainykController : BasicController, IAttack
{
    protected NetworkObject destroyObj;
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

                Invoke("DestroyParticle", 1.5f);
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
                Vector3 interpolation = new Vector3(0f, 2.5f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillEPreFeb, transform.position - interpolation, Quaternion.identity);
                obj.gameObject.transform.SetParent(gameObject.transform, true);
                destroyObj = obj;
                obj.gameObject.SetActive(true);

                AnimName = "WelcomeZone";
                Invoke("SettingAnimationIdle", 4f);


                Invoke("OffE", 4f);
                Invoke("DestroyParticle", 4f);
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
                Vector3 interpolation = new Vector3(0f, 2.5f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillQPreFeb, transform.position - interpolation, Quaternion.identity);
                obj.gameObject.transform.SetParent(gameObject.transform, true);
                destroyObj = obj;
                obj.gameObject.SetActive(true);

                AnimName = "Don'tCome";
                Invoke("SettingAnimationIdle", 1.2f);


                Invoke("OffQ", 1.2f);
                Invoke("DestroyParticle", 1.2f);
                isQAble = false;
                currentETime = stat.GetQTime();
            }
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

    void DestroyParticle()
    {
        NetworkManager.Instance.runner.Despawn(destroyObj);
    }
}
