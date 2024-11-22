using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

public class TyneyaController : BasicController, IAttack
{
    protected NetworkObject destroyObj;

    protected float eSKillTIme = 0f;
    protected bool eSkillOn = false;
    protected FrontSkill objSkill;

    protected bool qSkillOn = false;

    protected float damageInterpolation = 0f;

    protected override void Start()
    {
        base.Start();

    }
    protected override void InputActionW()
    {
        if (isWAble)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3 interpolation = new Vector3(0f, 0f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillWPreFeb, transform.position - interpolation, Quaternion.identity);
                obj.gameObject.transform.SetParent(gameObject.transform, true);
                destroyObj = obj;

                stat.SetSpeed(stat.GetSpeed()*1.02f);

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
            float x_interpolation = isLeft ? 5f : -4f;
            Vector3 interpolation = new Vector3(x_interpolation, 0f, 0);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillEPreFeb, transform.position - interpolation, Quaternion.identity);
            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            eSkillOn = true;
            damageInterpolation = 50f;
            stat.SetCurrentHp(stat.GetCurrentHp() / 2);

            isEAble = false;
            currentETime = stat.GetETime();

            Attack2();
        }
    }

    protected override void InputActionQ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            qSkillOn = true;
            damageInterpolation = 100f;
            Attack2();

            if (eSkillOn) damageInterpolation = 50f;
            else damageInterpolation = 0f;

            isQAble = false;
            currentQTime = stat.GetQTime();
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


    protected override void Attack2() // Tyneya, Mixube :근접 공격 구현 해 놓을 것
    {
        if (Input.GetMouseButtonDown(0))
        {
            float xinterpolation = isLeft ? 5f : -4f;
            Vector3 interpolation = new Vector3(xinterpolation, 1f, 0);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(BasicAttack, transform.position - interpolation, Quaternion.identity);
            objSkill = obj.gameObject.GetComponent<FrontSkill>();

            objSkill.SetDamage(objSkill.GetDamage() + damageInterpolation);
            qSkillOn = false;

            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            AnimName = "attack";
            Invoke("SettingAnimationIdle", 1.2f);

            Invoke("DestroyParticle", 1.2f);

        }
    }

    void IAttack.GetDamage(float Damage)
    {
        GameState Instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        float CurrentHp = stat.GetCurrentHp();
        stat.SetCurrentHp(CurrentHp - Damage);
        Instance.RPC_SetHP(stat.clientIndex, stat.GetCurrentHp(), stat.GetMaxHp());
        Object.RequestStateAuthority(); // State Authority 회복
    }

    void DestroyParticle()
    {
        objSkill.SetDamage(objSkill.GetDamage() - damageInterpolation);
    }

    protected override void ApplySkillEffect()
    {
        // under : E construction
        if(eSkillOn) eSKillTIme += NetworkManager.Instance.runner.DeltaTime;
        if(eSKillTIme > 10f)
        {
            eSkillOn = false;
            eSKillTIme = 0f;
        }
    }

}
