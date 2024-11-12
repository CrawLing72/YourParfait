using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine; // 시네머신 네임스페이스 추가

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 1200f;

    [SerializeField]
    float moveSpeed = 100.0f;

    [SerializeField]
    CinemachineVirtualCamera cinemachineCamera; // 시네머신 카메라 변수 선언

    private void LateUpdate()
    {
        CameraMove();
        CharacterLocationFix();
        CameraZoom();
    }

    protected void CameraZoom()
    {
        float zoomDirection = Input.GetAxis("Mouse ScrollWheel");
        var lensSettings = cinemachineCamera.m_Lens;

        // 줌 인/아웃 조정
        lensSettings.OrthographicSize -= zoomDirection * zoomSpeed * Time.deltaTime;
        lensSettings.OrthographicSize = Mathf.Clamp(lensSettings.OrthographicSize, 2.5f, 6f); // 줌 범위 설정
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
