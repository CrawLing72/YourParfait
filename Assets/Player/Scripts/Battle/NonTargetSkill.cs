using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;

public class NonTargetSkill : NonTargetThrow
{
    Stat playerStat;

    [SerializeField]
    float speed;

    [SerializeField]
    float time;

    float timer = 0f;
    float collisionTimer = 0f;
    float detection_timer = 0.2f; // ��� ���� 200ms �������� ���� (���ʿ� 200������ ����)
    bool onDetectionPlayer = false;

    public CircleCollider2D hitCollider;
    /*
    bool silent = false;
    float silentTime;

    bool slow = false;
    float slowTime;
    float slowValue;
    */


//    private float damage;

    Vector2 dir;

    private void Awake()
    {
        hitCollider = GetComponent<CircleCollider2D>();
        

    }

    public override void FixedUpdateNetwork()
    {
        timer += NetworkManager.Instance.runner.DeltaTime;
        collisionTimer += NetworkManager.Instance.runner.DeltaTime;
        transform.Translate(dir * speed * NetworkManager.Instance.runner.DeltaTime);

        if (timer > time)
        {
            Despawn();
        }
        if (collisionTimer > 0.3f)
        {
            hitCollider.enabled = true;
        }

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
                string tag = targetObj.gameObject.tag;
                switch (tag)
                {
                    case "Player":
                        gameState.Rpc_ApplyDamageAndEffects(targetObj.StateAuthority, damage, silent, silentTime, slow, slowValue, slowTime);
                        Despawn();
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
                        Despawn();
                        break;
                    case "Mob":
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if (mola != null) mola.Rpc_Damage(damage);
                        else if(tree != null) tree.Rpc_Damage(damage);

                        Despawn();
                        break;
                    default:
                        break;

                }

            }
            else if(targetObj != null) // -> ������ ������ Ŭ���̾�Ʈ�� ���, Ȥ�� �ش� Obj�� StateAuthority�� ������ �ִ� ���
            {
                string tag = targetObj.gameObject.tag;
                switch (tag)
                {
                    case "Player":
                        target.GetDamage(damage);
                        Despawn();
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
                        Despawn();
                        break;
                    case "Mob":
                        Debug.LogError("Mob Collision Detected!");
                        Mola mola = targetObj.gameObject.GetComponent<Mola>();
                        TreeMob tree = targetObj.gameObject.GetComponent<TreeMob>();

                        if (mola != null) mola.health -= damage;
                        else if(tree != null) tree.health -= damage;
                        Despawn();
                        break;
                    default:
                        break;

                }
            }
        }
    }

    public void SetStat(Stat stat)
    {
        playerStat = stat;
    }


    public void SetDirection(Vector2 diretion)
    {
        dir = diretion;
    }

    public void Despawn()
    {
        NetworkManager.Instance.runner.Despawn(Object);
    }

    public void SetTime(float limitTime)
    {
        time = limitTime;
    }

}
