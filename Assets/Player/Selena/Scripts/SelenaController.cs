using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelenaController : BasicController
{
    [Header("Skill")]
    [SerializeField]
    GameObject BasicAttack;

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

    private bool shouldFire = false;
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
            Debug.Log("Left Mouse Click");
            lastMouseClickPosition = Input.mousePosition;
            lastMouseClickPosition.z = Mathf.Abs(cam.transform.position.z);
            lastMouseClickPosition = cam.ScreenToWorldPoint(lastMouseClickPosition);
            shouldFire = true; // �߻� �÷��� ����
        }
    }

    protected void MouseLeftClick(Vector3 mouseClickPos) //-> ��Ÿ �θ�Ŭ���� ���� ���� �س��� �� : 11�� 11���� ����
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseClickPos, Vector2.zero, Mathf.Infinity);

        if (hit.collider != null)
        {
            Debug.Log($"Hit Object: {hit.collider.name}");

            Vector2 objectPos = gameObject.transform.position;
            Vector2 distance = ((Vector2)mouseClickPos - objectPos);

            Debug.Log($"Distance: {distance.magnitude}, Required Range: {stat.GetAttackRange()}");

            if (distance.magnitude < stat.GetAttackRange() && isAttackAble)
            {
                Vector3 projectilePos = gameObject.transform.position;
                projectilePos.x += (distance.x < 0 ? -1 : 1);
                projectilePos.y -= 1;

                GameObject projectile = Instantiate(BasicAttack, projectilePos, Quaternion.identity);
                projectile.GetComponent<NonTargetSkill>().GetDirection(distance.normalized);

                Debug.Log("Projectile Launched");

                isAttackAble = false;
                currentAttackTime = stat.GetAttackTime();
                shouldFire = false; // �߻� �� �÷��� �ʱ�ȭ
            }
        }
        else
        {
            Debug.Log("No Object Hit Detected");
            shouldFire = false; // �߻� ���� �� �÷��� �ʱ�ȭ
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
