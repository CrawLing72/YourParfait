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
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj?.GetComponent<IAttack>();

            if (targetObj != null && !targetObj.HasStateAuthority) // 본인이 마스터 클라이언트가 아닌 경우
            {
                string tag = targetObj.gameObject.tag;
                switch (tag)
                {
                    case "Player":
                        gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, silent, silentTime, slow, slowValue, slowTime);
                        Destroy();
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
                        Destroy();
                        break;
                    case "Mob":
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if (mola != null) mola.Rpc_Damage(damage);
                        else if (tree != null) tree.Rpc_Damage(damage);
                        Destroy();
                        break;
                    case "Table":
                        CraftingTable table = targetObj.gameObject.GetComponent<CraftingTable>();
                        if (table != null)
                        {
                            table.Rpc_PlusDamage(damage);
                        }
                        Destroy();
                        break;
                    default:
                        break;

                }

            }
            else if (targetObj != null) // -> 본인이 마스터 클라이언트인 경우, 혹은 해당 Obj에 StateAuthority를 가지고 있는 경우
            {
                string tag = targetObj.gameObject.tag;
                switch (tag)
                {
                    case "Player":
                        target.GetDamage(damage);
                        Destroy();
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
                        Destroy();
                        break;
                    case "Mob":
                        Debug.LogError("Mob Collision Detected!");
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if (mola != null) mola.health -= damage;
                        else if (tree != null) tree.health -= damage;
                        Destroy();
                        break;
                    case "Table":
                        CraftingTable table = targetObj.gameObject.GetComponent<CraftingTable>();
                        table.GetDamage(damage);
                        Destroy();
                        break;
                    default:
                        break;

                }
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
