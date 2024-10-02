using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadLoadingScene()
    {
        PlayerPrefs.SetString("NewScene", "Lobby");
        SceneManager.LoadScene("LoadingScene");
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void Start()
    {
        Debug.Log("MenuManager Start");
    }
}
