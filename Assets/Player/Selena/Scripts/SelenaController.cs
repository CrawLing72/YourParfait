using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelenaController : BasicController
{

    [Header("Status")]
    public float maxHp;
    public float maxMp;
    public float currentHp;
    public float currentMp;
    public float speed;
    public float attackRang;
    public float attackTime;
    public float wTime;
    public float eTime;
    public float rTime;
    public float ad;
    private Vector3 lastMouseClickPosition;

    protected override void Start()
    {
        base.Start();
        stat.SetMaxHp(maxHp);
        stat.SetMaxMp(maxMp);
        stat.SetCurrentHp(currentHp);
        stat.SetCurrentMp(currentMp);
        stat.SetSpeed(speed);
        stat.SetAttackRange(attackRang);
        stat.SetAttackTime(attackTime);
        stat.SetTWime(wTime);
        stat.SetTEime(eTime);
        stat.SetTRime(rTime);
        stat.SetAd(ad);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMouseClickPosition = Input.mousePosition;
            lastMouseClickPosition.z = Mathf.Abs(cam.transform.position.z);
            lastMouseClickPosition = cam.ScreenToWorldPoint(lastMouseClickPosition);
            shouldFire = true; // 발사 플래그 설정
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (shouldFire)
        {
            MouseLeftClick(lastMouseClickPosition);
        }
        base.FixedUpdateNetwork();
    }
}
