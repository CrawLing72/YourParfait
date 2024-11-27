using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickEffectVideo : MonoBehaviour
{
    public GameObject videoImagePrefab; // ���� ������
    public Animator Animator; // �ִϸ�����
    public Canvas canvas; // UI ĵ����
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowVideoAtClickPosition(Input.mousePosition, 0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ShowVideoAtClickPosition(Input.mousePosition, 1);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if(hit.collider != null)
        {
            string objName;
            if(hit.collider.gameObject.TryGetComponent<ObjDetInfo>(out var Obj))
            {
                objName = Obj.ObjName;
            }
            else
            {
                GameUIManager.instance.OffObjDescription();
                return;
            }

            if(objName != null && hit.collider.gameObject.CompareTag("NPC")) {

                GameUIManager.instance.SetObjDescription(objName);
            }
            else if (objName != null && hit.collider.gameObject.CompareTag("Mob"))
            {
                GameUIManager.instance.SetObjDescription(objName);
            }
            else if (objName != null && hit.collider.gameObject.CompareTag("Factory"))
            {
                GameUIManager.instance.SetObjDescription(objName);
            }
            else if (objName != null && hit.collider.gameObject.CompareTag("Table"))
            {
                GameUIManager.instance.SetObjDescription(objName);
            }
        }
    }

    void ShowVideoAtClickPosition(Vector2 screenPosition, int key)
    {
        // videoImagePrefab �ν��Ͻ� ����
        GameObject videoInstance = Instantiate(videoImagePrefab, canvas.transform);
        RectTransform videoRectTransform = videoInstance.GetComponent<RectTransform>();
        Animator videoAnimator = videoInstance.GetComponent<Animator>();

        // UI ��ǥ�� ��ȯ
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
