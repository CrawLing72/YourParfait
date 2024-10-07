using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public class Token
    {
        public string access_token;
    }

    [Header("UI Elements")]
    public GameObject loginButton;
    public GameObject lobbyButton;
    public GameObject mainMenu;
    public GameObject logginMenu;
    public TMP_Text loginAlert;

    public void LoadLoadingScene()
    {
        PlayerPrefs.SetString("NewScene", "Lobby");
        SceneManager.LoadScene("LoadingScene");
    }

    public void ActivateLoginMenu()
    {
        mainMenu.SetActive(false);
        logginMenu.SetActive(true);
    }

    public void DeactivateLoginMenu()
    {
        mainMenu.SetActive(true);
        logginMenu.SetActive(false);
    }

    public async void sendLogginData()
    {
        string name = GameObject.Find("identityField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("passwordField").GetComponent<TMP_InputField>().text;

        await APIManager.instance.sendJsonData(name, password, "login");
        long response_code = APIManager.instance.answered_data.response_code;
        if (response_code == 200)
        {
            DataManager.Instance.isLogined = true;
            Token token = JsonUtility.FromJson<Token>(APIManager.instance.answered_data.message);
            DataManager.Instance.JWTToken = token.access_token;
            DeactivateLoginMenu();

        } else if (response_code == 401)
        {
            loginAlert.text = "사용자가 없거나 비밀번호가 틀렸습니다.";
        } else if (response_code == 405)
        {
            loginAlert.text = "이미 존재하는 아이디 입니다.";
        } else if (response_code == 500)
        {
            loginAlert.text = "SERVER CLOSED";
        }
        else
        {
            loginAlert.text = "response_code : " + response_code.ToString();
        }
    }

    public void sendRegister()
    {
        string name = GameObject.Find("identityField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("passwordField").GetComponent<TMP_InputField>().text;

        APIManager.instance.sendJsonData(name, password, "register");
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 존재하면 새로 생성된 객체 파괴
        }
    }

    private void Update()
    {
        if (DataManager.Instance.isLogined)
        {
            loginButton.SetActive(false);
            lobbyButton.SetActive(true);
        }
        else
        {
            loginButton.SetActive(true);
            lobbyButton.SetActive(false);
        }
    }
}
