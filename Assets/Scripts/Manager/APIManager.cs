using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Threading.Tasks;

public class APIManager : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public string username;
        public string password;
    }

    public class GameOverData
    {
        public string username;
        public string password;
        public string target_server_id;
    }

    public class Message
    {
        public long response_code;
        public string message;
        public bool isError;
    }

    public static APIManager instance;
    public Message answered_data;
    public string username;


    private string apiUrl = "http://yourparfait.duckdns.org:5000";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public async Task sendJsonData(string _name, string _password, string keyword)
    {
        UserData userData = new UserData() { username = _name, password = _password };
        DataManager.Instance.username = _name;
        string json = JsonUtility.ToJson(userData);
        string fullUrl = apiUrl + "/" + keyword;

        string method = "POST";
        if (keyword == "matchmaking" || keyword == "gameover")
        {
            method = "GET";
        }

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, method))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            if (keyword == "matchmaking" || keyword == "gameover")
            {
                request.SetRequestHeader("Authorization", "Bearer " + DataManager.Instance.JWTToken);
            }

            var operation = request.SendWebRequest();

            // 응답이 올 때까지 비동기 대기
            while (!operation.isDone)
                await Task.Yield();  // 대기 시 유니티 메인 스레드를 차단하지 않음

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                answered_data = new Message() { response_code = request.responseCode, message = request.downloadHandler.text, isError = true };
            }
            else
            {
                answered_data = new Message() { response_code = request.responseCode, message = request.downloadHandler.text, isError = false };
            }
        }
    }

    public async Task sendJsonData_GameOver(string _name, string _password, string _servername)
    {
        GameOverData userData = new GameOverData() { username = _name, password = _password, target_server_id = _servername };
        DataManager.Instance.username = _name;
        string json = JsonUtility.ToJson(userData);
        string fullUrl = apiUrl + "/" + "gameover";

        string method = "GET";

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, method))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + DataManager.Instance.JWTToken);

            var operation = request.SendWebRequest();

            // 응답이 올 때까지 비동기 대기
            while (!operation.isDone)
                await Task.Yield();  // 대기 시 유니티 메인 스레드를 차단하지 않음

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                answered_data = new Message() { response_code = request.responseCode, message = request.downloadHandler.text, isError = true };
            }
            else
            {
                answered_data = new Message() { response_code = request.responseCode, message = request.downloadHandler.text, isError = false };
            }
        }
    }
}