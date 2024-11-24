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
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj?.GetComponent<IAttack>();

            if (targetObj != null && !targetObj.HasStateAuthority) // ������ ������ Ŭ���̾�Ʈ�� �ƴ� ���
            {
                // RPC ȣ��� ������ �� ���� �̻� ����
                if (!targetObj.gameObject.CompareTag("NPC")) gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, silent, silentTime, slow, slowValue, slowTime);
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
            }
            else // -> ������ ������ Ŭ���̾�Ʈ�� ���
            {
                if(targetObj != null && targetObj.gameObject.CompareTag("NPC")){
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
                else if (targetObj != null)
                {
                    target.GetDamage(damage);
                }
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
