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
    private async Task<bool> WaitForStateAuthorityWithTimeout(NetworkObject obj, int timeoutMs)
    {
        int elapsed = 0;
        while (!obj.HasStateAuthority && elapsed < timeoutMs)
        {
            await Task.Delay(10);
            elapsed += 10;
        }
        return obj.HasStateAuthority;
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Collision on!");    
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj.gameObject.GetComponent<IAttack>();

            if (targetObj != null && (!targetObj.HasStateAuthority)) // For preventing Self-kill
            {
                // StateAuthority 요청
                targetObj.RequestStateAuthority();

                // StateAuthority가 획득될 때까지 대기
                await WaitForStateAuthorityWithTimeout(targetObj, 150); // Maximum 150ms

                if (target != null && targetObj.HasStateAuthority)
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
                    targetObj.ReleaseStateAuthority();
                    Despawn();
                }
                else
                {
                    Debug.LogError("NO STATE AUTHORITY!!!");
                }
            }
            else
            {
                if(targetObj == null) Debug.LogError("Collision 대상에 NetworkObject가 없습니다!");
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
