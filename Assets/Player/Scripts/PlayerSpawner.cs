using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public GameObject[] UICAN;
    public GameObject WaitingText;
    public Vector3 SpawnPoint = new Vector3(-33, 2, -4);

    [Header("Chars")]
    [SerializeField]
    GameObject Selena;
    [SerializeField]
    GameObject Seraphina;
    [SerializeField]
    GameObject Mixube;
    [SerializeField]
    GameObject Tyneya;
    [SerializeField]
    GameObject Rainyk;

    PlayerSpawner playerSpawner;
    NetworkObject plObj;
    Stat plStat;
    int char_name;

    private void Awake()
    {
        playerSpawner = gameObject.GetComponent<PlayerSpawner>();
        string char_name = PlayerPrefs.GetString("CharName");
        Debug.Log("CharName: " + char_name);
        switch (char_name)
        {
            case "Selena":
                playerSpawner.PlayerPrefab = Selena;
                break;
            case "Seraphina":
                playerSpawner.PlayerPrefab = Seraphina;
                break;
            case "Mixube":
                playerSpawner.PlayerPrefab = Mixube;
                break;
            case "Tyneya":
                playerSpawner.PlayerPrefab = Tyneya;
                break;
            case "Rainyk":
                playerSpawner.PlayerPrefab = Rainyk;
                break;
            default:
                playerSpawner.PlayerPrefab = Selena;
                break;
        }
        Debug.Log("PlayerPrefab: " + playerSpawner.PlayerPrefab);
    }

    void Start()
    {
    }

    public void PlayerJoined(PlayerRef player) // IF로 안들어 오는 현상 있어서 임시 수정 : 나중에 땜빵 할 것
    {
        // Check if this is the local player
        if (player == NetworkManager.Instance.runner.LocalPlayer)
        {

            // Spawn the player object
            plObj = NetworkManager.Instance.runner.Spawn(PlayerPrefab, SpawnPoint, Quaternion.identity);
            if (plObj == null)
            {
                Debug.LogError("Failed to spawn player object!");
                return;
            }

            // Set the player object for the runner
            NetworkManager.Instance.runner.SetPlayerObject(player, plObj);

            // Determine character index
            switch (PlayerPrefs.GetString("CharName"))
            {
                case "Selena": char_name = 1; break;
                case "Seraphina": char_name = 2; break;
                case "Mixube": char_name = 3; break;
                case "Tyneya": char_name = 4; break;
                case "Rainyk": char_name = 0; break;
                default: char_name = 1; break;
            }

            // Ensure Stat component is present
            plStat = plObj.GetComponent<Stat>();
            if (plStat == null)
            {
                Debug.LogError("Stat component not found on PlayerPrefab!");
                return;
            }
        }

        // Activate UI elements
        foreach (var obj in UICAN)
        {
            obj.SetActive(true);
        }
        WaitingText.SetActive(false);

        SettingInfos(); 

        GameUIManager.instance.UpdateMainBar(true);
        GameUIManager.instance.UpdatePlayerStatus(true);
    }

    public void SettingInfos()
    {
        GameManager instance = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        // Set Synced Variables
        int clientIndex = PlayerPrefs.GetInt("ClientIndex");
        instance.Players_Char_Index.Set(clientIndex, char_name);
        instance.IsRedTeam_Sync.Set(clientIndex, instance.isRedTeam); // Default to Blue team
        instance.HP.Set(clientIndex, plStat.GetCurrentHp());
        instance.MaxHP.Set(clientIndex, plStat.GetMaxHp());
        instance.MP.Set(clientIndex, plStat.GetCurrentMp());
        instance.MaxMP.Set(clientIndex, plStat.GetMaxMp());

        Debug.LogError("SettingInfos: " + instance.IsRedTeam_Sync.Get(clientIndex).ToString());
    }

}