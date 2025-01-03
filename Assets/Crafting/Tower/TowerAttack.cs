using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttack : NetworkBehaviour
{
    [SerializeField]
    float attack;

    [SerializeField]
    GameObject attackPrefeb;

    
    bool bIsAttaclAble = false;

    bool bIsPlayer = false;
    bool bIsTarget = false;
    bool bIsAttacking = false;

    GameObject attackTarget;


    [SerializeField]
    bool isRedTeam = true;



    
    public void FixedUpdate()
    {
        
        
        if (bIsAttaclAble)
        {
            if (bIsTarget)
            {

                NetworkObject attackBall = NetworkManager.Instance.runner.Spawn(attackPrefeb, gameObject.transform.position, Quaternion.identity, Object.StateAuthority);
                RangeAttack rnageAttackc = attackBall.gameObject.GetComponent<RangeAttack>();

                attackBall.transform.position = gameObject.transform.position;

                rnageAttackc.SetSkillDamage(attack);
                rnageAttackc.GetTarget(attackTarget);

                bIsAttaclAble = false;
                Invoke("AttackTimer", 1);

            }
            else
            {
                bIsAttaclAble = false;
                bIsAttacking = false; 

            }
            
        }
        else
        {
            if (bIsTarget && !bIsAttacking)
            {
                Invoke("AttackTimer", 2);
                bIsAttacking = true;
            }
        }
        
    }

    

    

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!bIsPlayer)
        {
            BasicController Player = collision.GetComponent<BasicController>();
            if (Player != null && (isRedTeam != Player.isRedTeam))
            {
                bIsTarget = true;
                bIsPlayer = true;

                attackTarget = collision.gameObject;
            }
        }
        else
        {
            if (!bIsTarget)
            {
                BasicController Player = collision.GetComponent<BasicController>();
                if (Player != null)
                {
                    bIsTarget = true;
                    bIsPlayer = true;

                    attackTarget = collision.gameObject;
                }
                else
                {
                    MinionTemp minion = collision.GetComponent<MinionTemp>();
                    if(minion != null)
                    {
                        bIsTarget = true;
                        attackTarget = collision.gameObject;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BasicController Player = collision.GetComponent<BasicController>();
        if (Player != null)
        {
            bIsPlayer = false;
            bIsTarget = false;

        }
        else
        {
            MinionTemp minion = collision.GetComponent<MinionTemp>();
            if (minion != null)
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
