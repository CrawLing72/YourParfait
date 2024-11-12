using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixubeController : BasicController
{
    [SerializeField]
    protected GameObject attackPrefeb;

    [SerializeField]
    protected GameObject butterflyPrefb;

    [SerializeField]
    protected GameObject skillEEffect;

    [SerializeField]
    protected GameObject skillREffect;

    bool wIsOn = false;

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

    }

    protected override void InputActionW()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {


        }

    }

    protected override void InputActionE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }

    }

    protected override void InputActionR()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

        }

    }
}
