using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Rendering;
using UnityEditor.ShaderGraph.Internal;
#endif

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 300f;

    [SerializeField]
    float zoomMAx = 200f;

    [SerializeField]
    float moveSpeed = 100.0f;

    Camera mainCamera;

    protected void Awake()
    {
        mainCamera = Camera.main;
    }



    private void LateUpdate()
    {
        CameraMove();
        CharacterLocationFix();
    }

    protected void CameraZoom()
    {
        float zoonDirection = Input.GetAxis("Mouse ScrollWhell");
    }

    protected void CharacterLocationFix()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Vector2 pos = gameObject.transform.position;

            mainCamera.transform.position = new Vector3(pos.x,pos.y, -30.0f);
        }
    }

    protected void CameraMove()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = mainCamera.ScreenToViewportPoint(mousePos);

        if (mousePos.x < 0.01)
        {
            mainCamera.transform.Translate((new Vector2(-1.0f,0.0f)) * Time.deltaTime * moveSpeed);
        }
        else if(mousePos.x > 0.99)
        {
            mainCamera.transform.Translate((new Vector2(1.0f, 0.0f)) * Time.deltaTime * moveSpeed);
        }
        else if(mousePos.y < 0.01)
        {
            mainCamera.transform.Translate((new Vector2(0.0f, -1.0f)) * Time.deltaTime * moveSpeed);
        }
        else if(mousePos.y > 0.99)
        {
            mainCamera.transform.Translate((new Vector2(0.0f, 1.0f)) * Time.deltaTime * moveSpeed);
        }


    }

    

}
