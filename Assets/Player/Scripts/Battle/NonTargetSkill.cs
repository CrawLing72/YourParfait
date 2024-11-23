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
    float detection_timer = 0.2f; // 평균 핑이 200ms 내외임을 감안 (애초에 200넘으면 씹힘)
    bool onDetectionPlayer = false;

    CircleCollider2D hitCollider;
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

    // Collision Detection Part : 아래 구조는 수정시 대참사 일어 날 수 있으니 PM에게 무조건 문의

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        }
        else
        {
            Debug.LogError("Collision 대상에 NetworkObject가 없습니다!");
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
