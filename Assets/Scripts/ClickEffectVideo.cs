using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickEffectVideo : MonoBehaviour
{
    public GameObject videoImagePrefab; // 영상 프리팹
    public Animator Animator; // 애니메이터
    public Canvas canvas; // UI 캔버스
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowVideoAtClickPosition(Input.mousePosition, 0);
        }else if (Input.GetMouseButtonDown(1))
        {
            ShowVideoAtClickPosition(Input.mousePosition, 1);
        }   
    }

    void ShowVideoAtClickPosition(Vector2 screenPosition, int key)
    {
        // videoImagePrefab 인스턴스 생성
        GameObject videoInstance = Instantiate(videoImagePrefab, canvas.transform);
        RectTransform videoRectTransform = videoInstance.GetComponent<RectTransform>();
        Animator videoAnimator = videoInstance.GetComponent<Animator>();

        // UI 좌표로 변환
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            uiCamera,
            out Vector2 localPoint
        );

        videoRectTransform.localPosition = localPoint;
        videoAnimator.SetBool("IsLeft", key == 0 ? true : false);
        videoInstance.SetActive(true);

        Destroy(videoInstance, 1.5f);
    }
}
