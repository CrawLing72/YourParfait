using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : NonTargetThrow
{
    GameObject target;
    protected Vector2 vel;

    override public void FixedUpdateNetwork()
    {

        Vector2 myLocation = gameObject.transform.position;
        Vector2 targetLocation = target.transform.position;

        vel = (targetLocation - myLocation).normalized;

        gameObject.transform.Translate(vel * 50.0f * NetworkManager.Instance.runner.DeltaTime);


        //Debug.Log(vel.ToString());

    }

    // Collision Detection Part : �Ʒ� ������ ������ ������ �Ͼ� �� �� ������ PM���� ������ ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj.GetComponent<IAttack>();

            if (targetObj != null)
            {
                // RPC ȣ��� ������ �� ���� �̻� ����
                Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, targetObj, damage, silent, silentTime, slow, slowValue, slowTime);
                // ��ź ����
                Destroy();
            }
            else
            {
                 Debug.LogError("Collision ��� NetworkObject�� �����ϴ�!");
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_ApplyDamageAndEffects(
        [RpcTarget] PlayerRef player,
        NetworkObject targetObj,
        float damage,
        bool silent,
        float silentTime,
        bool slow,
        float slowValue,
        float slowTime
        )
    {
        IAttack target = targetObj.GetComponent<IAttack>();
        if (target != null)
        {
            target.GetDamage(damage);

            if (silent)
            {
                target.GetSilent(silentTime);
            }

            if (slow)
            {
                target.GetSlow(slowValue, slowTime);
            }
        }
    }

    public void GetTarget(GameObject Target)
    {
        target = Target;
    }

    public void Destroy()
    {
        NetworkManager.Instance.runner.Despawn(gameObject.GetComponent<NetworkObject>());
    }

}
