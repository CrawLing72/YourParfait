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

            if (targetObj != null && !targetObj.HasStateAuthority) // 본인이 마스터 클라이언트가 아닌 경우
            {

                string tag = targetObj.gameObject.tag;
                switch(tag)
                {
                    case "Player":
                        gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, silent, silentTime, slow, slowValue, slowTime);
                        break;
                    case "NPC":
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
                        break;
                    case "Mob":
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if(mola  != null) mola.Rpc_Damage(damage);
                        else if(tree != null) tree.Rpc_Damage(damage);
                        break;
                    default:
                        break;

                }
              
            }
            else if (targetObj != null) // -> 본인이 마스터 클라이언트인 경우
            {
                string tag = targetObj.gameObject.tag;
                switch (tag)
                {
                    case "Player":
                        target.GetDamage(damage);
                        break;
                    case "NPC":
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
                        break;
                    case "Mob":
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if (mola != null) mola.health -= damage;
                        else if(tree != null) tree.health -= damage;
                        break;
                    default:
                        break;

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
