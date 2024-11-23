using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundZAxis : MonoBehaviour
{
    public Transform center; // ȸ�� �߽��� �Ǵ� Transform
    public float radius = 2f; // ȸ�� ������
    public float speed = 2f; // ȸ�� �ӵ� (���ӵ�, rad/s)

    private float angle = 0f; // ���� ���� (���� ����)

    void Update()
    {
        // �ð��� ���� ���� ����
        angle += speed * Time.deltaTime;

        // ��������Ʈ�� �� ��ġ ���
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;

        // ������Ʈ ��ġ ������Ʈ
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
