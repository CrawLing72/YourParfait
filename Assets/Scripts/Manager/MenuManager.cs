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
    public bool isLoggined = false;

    [Header("UI Elements")]
    public GameObject loginButton;
    public GameObject lobbyButton;
    public GameObject mainMenu;
    public GameObject logginMenu;

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

    public void sendLogginData()
    {
        string name = GameObject.Find("identityField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("passwordField").GetComponent<TMP_InputField>().text;

        APIManager.instance.sendJsonData(name, password, "login");
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
        if (isLoggined)
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
