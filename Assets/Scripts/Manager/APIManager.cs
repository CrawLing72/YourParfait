using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIManager : MonoBehaviour
{
    // API ��������Ʈ URL -> Ŭ�� ���ο��� �����ϴ� ���̶� ���߿� GCP or Azure�� ������ ��
    private string apiUrl = "https://127.0.0.1:5000";

    void Start()
    {
        // �ڷ�ƾ�� ����Ͽ� �񵿱� ��û ����
        StartCoroutine(GetJsonData());
    }

    // �񵿱� �ڷ�ƾ �Լ�
    IEnumerator GetJsonData()
    {
        // UnityWebRequest ��ü ����
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        // ��û ������
        yield return request.SendWebRequest();

        // ���� üũ
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // ��û�� ���������� �Ϸ�Ǹ� JSON �����͸� ���
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Received: " + jsonResponse);

            // JSON �Ľ� �� ó�� (�ʿ��)
            // var data = JsonUtility.FromJson<YourClass>(jsonResponse);
        }
    }
}