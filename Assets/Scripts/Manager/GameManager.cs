using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : MonoBehaviour
{

    //GameManager singleton instance
    public static GameManager instance;

    //GameInfo instance
    public GameInfo gameInfo;

    private void Awake()
    {
        //singleton pattern implementation
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

        //Game Info Settings
        gameInfo = new GameInfo();
        gameInfo.ServerName = PlayerPrefs.GetString("Server");
        gameInfo.PlayerName = PlayerPrefs.GetString("Name");
        gameInfo.CurrentChar = PlayerPrefs.GetString("CharName");

    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        GameManager.instance.GetPlayerInfos();
    }

    public void GetPlayerInfos()
    {
        //Get other Player's Information
        IEnumerable<PlayerRef> Item = NetworkManager.Instance.runner.ActivePlayers;
        GameManager.instance.gameInfo.CurrentPlayers = NetworkManager.Instance.runner.SessionInfo.PlayerCount;
        foreach (Fusion.PlayerRef Player in Item) // Player들 처리하기!
        {
            Stat PlayerStat;
            if (NetworkManager.Instance.runner.TryGetPlayerObject(Player, out var plObject))
            {
                PlayerStat = plObject.gameObject.GetComponent<Stat>(); // Player의 Stat Component 불러오기
            }
            else
            {
                return;
            }

            Playerinfo temp_info = new Playerinfo();
            temp_info.SetParameters(gameInfo.PlayerName, gameInfo.isRedTeam, PlayerStat.GetCurrentHp(), PlayerStat.GetMaxHp(), PlayerStat.GetCurrentMp(), PlayerStat.GetMaxMp(),gameInfo.CurrentChar);
            gameInfo.Players[PlayerStat.gameManager.gameInfo.PlayerName] = temp_info;
        }
    }
}
