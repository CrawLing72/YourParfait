using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField]
    float attack;

    [SerializeField]
    GameObject attackPrefeb;

    GameObject target;

    bool bIsPlayer = false;
    bool bIsTarget = false;
    bool bIsAttaclAble = false;
    bool bIsMinion = false;

    private List<GameObject> PlayerList;
    private List<GameObject> MinionList;

    [SerializeField]
    bool team;

    public void Update()
    {
        

        if (bIsAttaclAble)
        {
            if (bIsTarget)
            {
                if (bIsPlayer)
                {
                    target = PlayerList[0];
                    if (target != null)
                    {
                        Debug.Log("Player");
                        GameObject attackBall = Instantiate(attackPrefeb);
                        RangeAttack rnageAttackc = attackBall.GetComponent<RangeAttack>();

                        attackBall.transform.position = gameObject.transform.position;

                        rnageAttackc.SetSkillDamage(attack);
                        rnageAttackc.GetTarget(target);


                        bIsAttaclAble = false;
                        Invoke("AttackTimer", 1);
                    }
                    else
                    {
                        PlayerList.RemoveAt(0);
                    }
                    
                }
                else
                {
                    target = MinionList[0];
                    if (target != null)
                    {
                        GameObject attackBall = Instantiate(attackPrefeb);
                        RangeAttack rnageAttackc = attackBall.GetComponent<RangeAttack>();

                        rnageAttackc.SetSkillDamage(attack);
                        rnageAttackc.GetTarget(target);

                        bIsAttaclAble = false;
                        Invoke("AttackTimer", 1);
                    }
                    else
                    {
                        PlayerList.RemoveAt(0);
                    }
                }
            }
        }
        else
        {
            if (bIsTarget)
            {
                Invoke("AttackTimer", 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IAttack inTarget = collision.gameObject.GetComponent<IAttack>();
        if (inTarget != null)
        {
            bIsTarget = true;

            MinionTemp minion = collision.gameObject.GetComponent<MinionTemp>();
            if (minion != null)
            {
                bIsMinion = true;
                MinionList.Add(collision.gameObject);
            }
            else
            {
                BasicController player = collision.gameObject.GetComponentInParent<BasicController>();
                if (player != null)
                {
                    bIsPlayer = true;
                    PlayerList.Add(collision.gameObject);

                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IAttack inTarget = collision.gameObject.GetComponent<IAttack>();

        if (inTarget != null)
        {
            MinionTemp minion = collision.gameObject.GetComponent<MinionTemp>();
            if (minion != null)
            {
                MinionList.Remove(collision.gameObject);
                if (MinionList.Count == 0)
                {
                    bIsMinion = false;
                }

            }
            else
            {
                BasicController player = collision.gameObject.GetComponentInParent<BasicController>();
                if (player != null)
                {
                    PlayerList.Remove(collision.gameObject);
                    if (PlayerList.Count == 0)
                    {
                        bIsPlayer = false;
                    }

                }
            }

            if (!bIsPlayer && !bIsMinion)
            {
                bIsTarget = false;
            }
        }
    }

    void AttackTimer()
    {
        bIsAttaclAble = true;
    }
}
