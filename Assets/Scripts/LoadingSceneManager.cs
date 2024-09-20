using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("Requirements")]

    public string nextScene;

    public RectTransform foxTrnasform;

    [Header("FoxMotionSettings")]
    public int startpoint;
    public int endpoint;

    bool isTime;

    // under is about fuctions
    void Start()
    {
        nextScene = PlayerPrefs.GetString("NewScene");
        Debug.Log(nextScene);
        StartCoroutine(LoadScene());
        isTime = false;
    }

    void timeSet()
    {
        isTime = true;
        Debug.Log("triggered!");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //�񵿱� �ڷ�ƾ ����
        op.allowSceneActivation = false; // ������ �ϴ� ��Ȱ��ȭ

        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.90f)
            {

                int pointIntervalValue = endpoint - startpoint;
                float translated_position_x = startpoint + ((float)pointIntervalValue * op.progress);

                Vector3 originalVector = foxTrnasform.anchoredPosition;
                originalVector.x = translated_position_x;
                foxTrnasform.anchoredPosition = originalVector; // ���������ϴ�

                Invoke(nameof(timeSet), 3f);
                op.allowSceneActivation = isTime;
                if (isTime)
                {
                    yield break;
                }
            }
            else
            {
                int pointIntervalValue = endpoint - startpoint;
                float translated_position_x = startpoint + ((float)pointIntervalValue * op.progress); //���̱ް� ���ϱ�

                Vector3 originalVector = foxTrnasform.anchoredPosition;
                originalVector.x = translated_position_x;
                foxTrnasform.anchoredPosition = originalVector; //���Ͱ� ���ͼ� ġȯ�� �ٽ� ����ֱ�
            }

        }


    }
}