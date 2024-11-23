using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixubeController : BasicController, IAttack
{
    protected NetworkObject destroyObj;
    protected PolygonCollider2D bakCollider;
    protected override void Start()
    {
        base.Start();
        bakCollider = GetComponent<PolygonCollider2D>();

    }
    protected override void InputActionW() // 기본 공격 외 구현 안됨
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
    protected override void InputActionE() // 기본 공격 외 구현 안됨
    {

    }

    protected override void InputActionQ()
    {
        if (isQAble)
        {
            if (Input.GetKeyDown(KeyCode.Q))
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

                AnimName = "attack";
                Invoke("SettingAnimationIdle", 1.66f);


                Invoke("OffQ", 1.66f);
                Invoke("DestroyParticle", 1.66f);
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


    protected override void Attack2() // Tyneya, Mixube :근접 공격 구현 해 놓을 것
    {
        if (Input.GetMouseButtonDown(0))
        {
            float xinterpolation = isLeft ? 1f : -0.5f;
            Vector3 interpolation = new Vector3(xinterpolation, 1f, 0);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(BasicAttack, transform.position - interpolation, Quaternion.identity);
            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            AnimName = "attack";
            Invoke("SettingAnimationIdle", 0.834f);

            Invoke("DestroyParticle", 0.834f);
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
        if(destroyObj != null)
            NetworkManager.Instance.runner.Despawn(destroyObj);
    }

}
