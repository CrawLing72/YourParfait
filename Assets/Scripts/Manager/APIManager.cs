using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIManager : MonoBehaviour
{
    // API 엔드포인트 URL -> 클라 내부에서 실험하는 중이라 나중에 GCP or Azure로 이전할 것
    private string apiUrl = "https://127.0.0.1:5000";

    void Start()
    {
        // 코루틴을 사용하여 비동기 요청 실행
        StartCoroutine(GetJsonData());
    }

    // 비동기 코루틴 함수
    IEnumerator GetJsonData()
    {
        // UnityWebRequest 객체 생성
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        // 요청 보내기
        yield return request.SendWebRequest();

        // 오류 체크
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 요청이 성공적으로 완료되면 JSON 데이터를 출력
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Received: " + jsonResponse);

            // JSON 파싱 및 처리 (필요시)
            // var data = JsonUtility.FromJson<YourClass>(jsonResponse);
        }
    }
}