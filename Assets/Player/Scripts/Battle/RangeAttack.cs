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
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj?.GetComponent<IAttack>();

            if (targetObj != null && !targetObj.HasStateAuthority) // ������ ������ Ŭ���̾�Ʈ�� �ƴ� ���
            {
                // RPC ȣ��� ������ �� ���� �̻� ����
                if (!targetObj.gameObject.CompareTag("NPC") && !targetObj.gameObject.CompareTag("mob"))
                {
                    Stat targetStat = targetObj.gameObject.GetComponent<Stat>();
                    targetStat.SetCurrentHp(targetStat.GetCurrentHp() - damage); // -> SetCurrentHP ���ο� RPC ����
                }
                else
                {
                    MinionsAIBlue targetMinion = targetObj.GetComponent<MinionsAIBlue>();
                    MinionsAIRed targetMinion_Red = targetObj.GetComponent<MinionsAIRed>();
                    if (targetMinion != null)
                    {
                        targetMinion.Rpc_Damage(damage);
                    }
                    else if (targetMinion_Red != null)
                    {
                        targetMinion_Red.Rpc_Damage(damage);
                    }
                }
                Destroy();
            }
            else // -> ������ ������ Ŭ���̾�Ʈ�� ���
            {
                if (targetObj != null && targetObj.gameObject.CompareTag("NPC"))
                {
                    if (targetObj.gameObject.CompareTag("Mob"))
                    {
                        MinionsAIBlue targetMinion = targetObj.GetComponent<MinionsAIBlue>();
                        MinionsAIRed targetMinion_Red = targetObj.GetComponent<MinionsAIRed>();
                        if (targetMinion != null)
                        {
                            targetMinion.HP -= damage;
                        }
                        else if (targetMinion_Red != null)
                        {
                            targetMinion_Red.HP -= damage;
                        }
                    }
                }
                else if (targetObj != null && targetObj.gameObject.CompareTag("Player"))
                {
                    target.GetDamage(damage);
                }
                Destroy();
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
