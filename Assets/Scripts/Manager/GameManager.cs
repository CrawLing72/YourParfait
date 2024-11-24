using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : MonoBehaviour
{
    public GameState gameState; //jghjikgjhjgk
    public bool isRedTeam = true;
    public GameObject ClientPlayer;

    public bool isGameStarted = true; // Build 때는 반드시 false로 해 놓을 것.
    public bool isGameOvered = false;

    public int Coins = 0;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }
    private void Update()
    {
        if(gameState.GSSpawned == 1)
        {
            GameUIManager.instance.UpdateMainBar(true);
            GameUIManager.instance.UpdateTopStatusBar();
            GameUIManager.instance.UpdatePlayerStatus(true);
        }

        // Game Starting Logic
        // Game Ended Logic
    }
}
