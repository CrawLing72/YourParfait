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

    // Collision Detection Part : 아래 구조는 수정시 대참사 일어 날 수 있으니 PM에게 무조건 문의
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
            if (collision != null)
            {
                NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
                IAttack target = targetObj?.GetComponent<IAttack>();

                if (targetObj != null) // Onlly For Towers : Player Range Attack이 필요하면 따로 상속받을것.
                {
                    // RPC 호출로 데미지 및 상태 이상 적용
                    gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, silent, silentTime, slow, slowValue, slowTime);

                    // 포탄 제거
                    Destroy();
                }
            }
            else
            {
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

    public void GetTarget(GameObject Target)
    {
        target = Target;
    }

    public void Destroy()
    {
        NetworkManager.Instance.runner.Despawn(gameObject.GetComponent<NetworkObject>());
    }

}
