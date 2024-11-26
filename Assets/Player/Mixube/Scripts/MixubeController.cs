using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixubeController : BasicController, IAttack
{
    protected NetworkObject destroyObj;
    protected PolygonCollider2D bakCollider;
    protected FrontSkill objSkill;
    protected override void Start()
    {
        base.Start();
        bakCollider = GetComponent<PolygonCollider2D>();

    }
    protected override void InputActionW() // �⺻ ���� �� ���� �ȵ�
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
    protected override void InputActionE() // �⺻ ���� �� ���� �ȵ�
    {

    }

    protected override void InputActionQ()
    {
        if (isQAble)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 interpolation = new Vector3(0f, 0f, 0);
                NetworkObject obj = NetworkManager.Instance.runner.Spawn(skillQPreFeb, - interpolation, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform, true);

                if (isRedTeam) obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("RedTeam");
                else obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("BlueTeam");

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


    protected override void Attack2() // Tyneya, Mixube :���� ���� ���� �� ���� ��
    {
        if (Input.GetMouseButtonDown(0))
        {
            float xinterpolation = isLeft ? 2f : -0.5f;
            Vector3 interpolation = new Vector3(xinterpolation, 1f, 0);
            NetworkObject obj = NetworkManager.Instance.runner.Spawn(BasicAttack, CurrenPosition - interpolation, Quaternion.identity);
            if (isRedTeam) obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("RedTeam");
            else obj.gameObject.GetComponent<FrontSkill>().hitCollider_polygon.excludeLayers = LayerMask.GetMask("BlueTeam");


            objSkill = obj.gameObject.GetComponent<FrontSkill>();

            if (!isLeft) obj.gameObject.transform.localScale = new Vector3(-1, 1, 1);

            objSkill.SetDamage(objSkill.GetDamage());

            obj.gameObject.transform.SetParent(gameObject.transform, true);
            destroyObj = obj;

            AnimName = "attack";
            Invoke("SettingAnimationIdle", 1.2f);

            Invoke("DestroyParticle", 1.2f);

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
        if(destroyObj != null)
            NetworkManager.Instance.runner.Despawn(destroyObj);
    }

}
