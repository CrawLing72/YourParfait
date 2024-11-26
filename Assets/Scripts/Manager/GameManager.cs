using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState gameState; //jghjikgjhjgk
    public bool isRedTeam = true;
    public GameObject ClientPlayer;

    public bool isGameStarted = false; // Build 때는 반드시 false로 해 놓을 것.
    public bool isGameOvered = false;
    public bool isRedTeamWin = true;

    public int Coins = 0;
    public int PlayerDet = 0;

    private float EndTimer = 0f;

    //singleton pattern implementation
    public static GameManager instance;
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
    }

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private async void WhenGameOvered()
    {
        await APIManager.instance.sendJsonData_GameOver(PlayerPrefs.GetString("Name"), PlayerPrefs.GetString("Password"), "matchmaking");
        long response_code = APIManager.instance.answered_data.response_code;
        if (response_code == 200)
        {
            PlayerPrefs.SetString("NewScene", "MainMenu");
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("Error : " + response_code);
            PlayerPrefs.SetString("NewScene", "MainMenu");
            SceneManager.LoadScene("LoadingScene");
        }

        isGameOvered = false;
        isGameStarted = true;
    }
    private void Update()
    {

        if((NetworkManager.Instance.runner.SessionInfo.PlayerCount) > PlayerDet)
        {
            isGameStarted = true;
        }
        if(gameState.GSSpawned == 1 && !isGameOvered)
        {
            GameUIManager.instance.UpdateMainBar(true);
            GameUIManager.instance.UpdateTopStatusBar();
            GameUIManager.instance.UpdatePlayerStatus(true);
            if(PlayerPrefs.GetInt("ClientIndex") < PlayerDet)
            {
                isRedTeam = true;
            }
            else{
                isRedTeam = false;
            }
        }

        // Game Starting Logic
        if (isGameOvered)
        {
            EndTimer += Time.deltaTime;
            GameUIManager.instance.Char_Face.transform.parent.gameObject.SetActive(false);
            GameUIManager.instance.RootObj.SetActive(false);
            GameUIManager.instance.KDA.transform.parent.gameObject.SetActive(false);
            GameUIManager.instance.Minimap.SetActive(false);

            GameUIManager.instance.DeadEffect.SetActive(true);
            GameUIManager.instance.RespawnTimeText.SetActive(true);

            GameUIManager.instance.RespawnTimeText.GetComponent<TMP_Text>().text = "Game Over! ";

            if (EndTimer > 7f)
            {
                WhenGameOvered();
            }

        }
    }
}
