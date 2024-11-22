using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
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

    private async Task OnTriggerEnter2DAsync(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player")) // "Player"�±װ� �޷� �־�߸��� �ǰ� ����
            {
                NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
                IAttack target = targetObj.gameObject.GetComponent<IAttack>();

                if (targetObj != null && (!targetObj.HasStateAuthority)) // For preventing Self-kill
                {
                    // StateAuthority ��û
                    targetObj.RequestStateAuthority();

                    // StateAuthority�� ȹ��� ������ ���
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
                    }
                    else
                    {
                        Debug.LogError("NO STATE AUTHORITY!!!");
                    }
                }
                else
                {
                    if (targetObj == null) Debug.LogError("Collision ��� NetworkObject�� �����ϴ�!");
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
