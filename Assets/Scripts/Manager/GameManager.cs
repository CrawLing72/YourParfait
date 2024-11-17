using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : MonoBehaviour
{
    GameState gameState;
    public bool isRedTeam = true;

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
    }
}
