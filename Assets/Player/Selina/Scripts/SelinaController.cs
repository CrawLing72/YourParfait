using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelinaController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    [SerializeField]
    protected GameObject skillWEffect;

    [SerializeField]
    protected GameObject skillEEffect;

    [SerializeField]
    protected GameObject skillREffect;


    protected override void Start()
    {
        base.Start();

    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();



    }

    protected override void Attack(GameObject Target)
    {
        GameObject attack = Instantiate(attackPrefeb);
        RangeAttack attackFunc = attack.GetComponent<RangeAttack>();

        attack.transform.position = gameObject.transform.position;
        attackFunc.GetTarget(Target, stat.GetAd());
    }

    protected override void InputActionW()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(isWAble)
            {

                currentWTime = stat.GetWTime();
            }

        }

    }

    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isEAble)
            {

            }
        }

    }

    protected override void InputActionR()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isRAble)
            {

            }
        }

    }
}
