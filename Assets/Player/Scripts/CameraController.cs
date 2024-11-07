using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine; // �ó׸ӽ� ���ӽ����̽� �߰�

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 300f;

    [SerializeField]
    float zoomMax = 200f;

    [SerializeField]
    float moveSpeed = 100.0f;

    [SerializeField]
    CinemachineVirtualCamera cinemachineCamera; // �ó׸ӽ� ī�޶� ���� ����

    private void LateUpdate()
    {
        CameraMove();
        CharacterLocationFix();
    }

    protected void CameraZoom()
    {
        float zoomDirection = Input.GetAxis("Mouse ScrollWheel");
        var lensSettings = cinemachineCamera.m_Lens;

        // �� ��/�ƿ� ����
        lensSettings.FieldOfView -= zoomDirection * zoomSpeed * Time.deltaTime;
        lensSettings.FieldOfView = Mathf.Clamp(lensSettings.FieldOfView, 15f, zoomMax); // �� ���� ����
        cinemachineCamera.m_Lens = lensSettings;
    }

    protected void CharacterLocationFix()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector2 pos = transform.position;
            cinemachineCamera.transform.position = new Vector3(pos.x, pos.y, -30.0f);
        }
    }

    protected void CameraMove()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToViewportPoint(mousePos);

        Vector3 direction = Vector3.zero;

        if (mousePos.x < 0.01f)
            direction = Vector3.left;
        else if (mousePos.x > 0.99f)
            direction = Vector3.right;
        else if (mousePos.y < 0.01f)
            direction = Vector3.down;
        else if (mousePos.y > 0.99f)
            direction = Vector3.up;

        cinemachineCamera.transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}
