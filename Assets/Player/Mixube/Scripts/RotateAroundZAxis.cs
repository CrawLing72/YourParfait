using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundZAxis : MonoBehaviour
{
    public Transform center; // 회전 중심이 되는 Transform
    public float radius = 2f; // 회전 반지름
    public float speed = 2f; // 회전 속도 (각속도, rad/s)

    private float angle = 0f; // 현재 각도 (라디안 단위)

    void Update()
    {
        // 시간에 따라 각도 증가
        angle += speed * Time.deltaTime;

        // 스프라이트의 새 위치 계산
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;

        // 오브젝트 위치 업데이트
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
