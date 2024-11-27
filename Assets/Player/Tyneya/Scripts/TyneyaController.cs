using AK.Wwise;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillWPreFeb, CurrenPosition - interpolation, Quaternion.identity, NetworkManager.Instance.runner.LocalPlayer);
                obj.gameObject.transform.SetParent(gameObject.transform, true);
                destroyObj = obj;

                stat.SetSpeed(stat.GetSpeed()*1.02f);

                Invoke("DestroyParticle", 1.5f);
                isWAble = false;
                currentWTime = stat.GetWTime();

                if (Object.HasStateAuthority) SpawnSoundPrefab("W");
                else Rpc_Sound("W");
            }
        }
    }
    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEAble)
        {
            Vector3 interpolation = new Vector3(0f, 0f, 0f);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillEPreFeb, CurrenPosition - interpolation, Quaternion.identity, NetworkManager.Instance.runner.LocalPlayer);
            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            eSkillOn = true;
            damageInterpolation = 50f;
            stat.SetCurrentHp(stat.GetCurrentHp() / 2);

            isEAble = false;
            currentETime = stat.GetETime();

            Attack2();

            if(Object.HasStateAuthority) SpawnSoundPrefab("E");
            else SpawnSoundPrefab("E");
        }
    }

    protected override void InputActionQ()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isQAble)
        {
            qSkillOn = true;
            damageInterpolation = 100f;
            Attack2();

            if (eSkillOn) damageInterpolation = 50f;
            else damageInterpolation = 0f;

            isQAble = false;
            currentQTime = stat.GetQTime();

            if(Object.HasStateAuthority) SpawnSoundPrefab("Q");
            else SpawnSoundPrefab("Q");
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
            float xinterpolation = isLeft ? 2f : -0.5f;
            Vector3 interpolation = new Vector3(xinterpolation, 1f, 0);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(BasicAttack, CurrenPosition - interpolation, Quaternion.identity, NetworkManager.Instance.runner.LocalPlayer);

            BAK_Close.Post(gameObject);

            if (isRedTeam) obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("RedTeam");
            else obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("BlueTeam");

            objSkill = obj.gameObject.GetComponent<FrontSkill>();

            if(!isLeft)obj.gameObject.transform.localScale = new Vector3(-1, 1, 1);

            objSkill.SetDamage(objSkill.GetDamage() + damageInterpolation);
            qSkillOn = false;

            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            AnimName = "attack";
            Invoke("SettingAnimationIdle", 1.2f);

            Invoke("DestroyParticle", 1.2f);

            if(Object.HasStateAuthority) SpawnSoundPrefab("BAK_Close");
            else SpawnSoundPrefab("BAK_Close");

        }
    }

    public new void GetDamage(float Damage)
    {
        GameState Instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        float CurrentHp = stat.GetCurrentHp();
        stat.SetCurrentHp(CurrentHp - Damage);
        Instance.RPC_SetHP(stat.clientIndex, stat.GetCurrentHp(), stat.GetMaxHp());
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
