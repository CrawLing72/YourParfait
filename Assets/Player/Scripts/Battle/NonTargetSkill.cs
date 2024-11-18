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
    float detection_timer = 0.2f; // 평균 핑이 200ms 내외임을 감안 (애초에 200넘으면 씹힘)
    bool onDetectionPlayer = false;
    /*
    bool silent = false;
    float silentTime;

    bool slow = false;
    float slowTime;
    float slowValue;
    */


//    private float damage;

    Vector2 dir;

    private void Start()
    {
        SetSkillDamage(50);
    }

    public override void FixedUpdateNetwork()
    {
        timer += NetworkManager.Instance.runner.DeltaTime;
        transform.Translate(dir * speed * NetworkManager.Instance.runner.DeltaTime);
        Debug.Log(timer);
        if (timer > time)
        {
            Despawn();
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
        if (collision != null)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            IAttack target = targetObj.gameObject.GetComponent<IAttack>();

            if (targetObj != null)
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
                Debug.LogError("Collision 대상에 NetworkObject가 없습니다!");
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

}
