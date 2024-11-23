using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FrontSkill : NetworkBehaviour
{
    [SerializeField]
    protected float damage;

    protected bool silent = false;
    protected float silentTime;

    protected bool slow = false;
    protected float slowTime;
    protected float slowValue;

    protected bool bisbondage;

    bool bTeam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Collision on!");
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj?.GetComponent<IAttack>();

            if (targetObj != null && !targetObj.HasStateAuthority) // Prevent self-kill
            {
                // RPC 호출로 데미지 및 상태 이상 적용
                Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, targetObj, damage, silent, silentTime, slow, slowValue, slowTime);

                // 포탄 제거
                Despawn();
            }
            else
            {
                if (targetObj == null)
                    Debug.LogError("Collision 대상에 NetworkObject가 없습니다!");
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

    public void Despawn()
    {
        NetworkManager.Instance.runner.Despawn(Object);
    }

    public void SetDamage(float _dm)
    {
        damage = _dm;
    }

    public float GetDamage()
    {
        return damage;
    }
}
