using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickEffectVideo : MonoBehaviour
{
    public GameObject videoImagePrefab; // ���� ������
    public Canvas canvas; // UI ĵ����
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowVideoAtClickPosition(Input.mousePosition);
        }
    }

    void ShowVideoAtClickPosition(Vector2 screenPosition)
    {
        // videoImagePrefab �ν��Ͻ� ����
        GameObject videoInstance = Instantiate(videoImagePrefab, canvas.transform);
        RectTransform videoRectTransform = videoInstance.GetComponent<RectTransform>();

        // UI ��ǥ�� ��ȯ
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            uiCamera,
            out Vector2 localPoint
        );

        videoRectTransform.localPosition = localPoint;
        videoInstance.SetActive(true);

        Destroy(videoInstance, 1.5f);
    }
}
