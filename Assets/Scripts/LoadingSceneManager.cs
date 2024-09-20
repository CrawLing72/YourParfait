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
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //비동기 코루틴 실행
        op.allowSceneActivation = false; // 다음신 일단 비활성화

        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.90f)
            {

                int pointIntervalValue = endpoint - startpoint;
                float translated_position_x = startpoint + ((float)pointIntervalValue * op.progress);

                Vector3 originalVector = foxTrnasform.anchoredPosition;
                originalVector.x = translated_position_x;
                foxTrnasform.anchoredPosition = originalVector; // 복붙했읍니다

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
                float translated_position_x = startpoint + ((float)pointIntervalValue * op.progress); //사이급간 더하기

                Vector3 originalVector = foxTrnasform.anchoredPosition;
                originalVector.x = translated_position_x;
                foxTrnasform.anchoredPosition = originalVector; //벡터값 떼와서 치환후 다시 집어넣기
            }

        }


    }
}