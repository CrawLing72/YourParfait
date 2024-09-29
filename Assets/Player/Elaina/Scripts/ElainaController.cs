using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElainaController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    protected override void Start()
    {
        base.Start(); 

        stat.SetMaxHp(1000.0f);
        stat.SetCurrentHp(1000.0f);
        stat.SetMaxMp(1000.0f);
        stat.SetCurrentMp(1000.0f);
        stat.SetAttackRange(10.0f);

        stat.SetAttackTime(1.0f);
        stat.SetTEime(10.0f);
        stat.SetTWime(10.0f);
        stat.SetTRime(10.0f);

    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
    }

    protected override void Attack(Vector2 targetLocation)
    {
        GameObject attack = Instantiate(attackPrefeb);
        attack.transform.position = targetLocation;
    }

    protected override void InputActionW()
    {

    }

    protected override void InputActionE()
    {

    }

    protected override void InputActionR()
    {

    }

    public override void GetDamage(float Damage)
    {
        Debug.Log("ElaniaDamage");
    }
}
