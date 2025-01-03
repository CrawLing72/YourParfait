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
    public Vector3 RedSpawnPoint = new Vector3(-33, 2, -4);
    public Vector3 BlueSpawnPoint = new Vector3(29.12f, 0f, -4);

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

    public void PlayerJoined(Fusion.PlayerRef player) // IF로 안들어 오는 현상 있어서 임시 수정 : 나중에 땜빵 할 것
    {
        // Check if this is the local player
        if (player == NetworkManager.Instance.runner.LocalPlayer)
        {
            // Spawn the player object
            plObj = NetworkManager.Instance.runner.Spawn(PlayerPrefab, RedSpawnPoint, Quaternion.identity);
            PlayerPrefs.SetInt("ClientIndex", NetworkManager.Instance.runner.SessionInfo.PlayerCount - 1);

            if(PlayerPrefs.GetInt("ClientIndex") < GameManager.instance.PlayerDet)
            {
                plObj.gameObject.transform.position = RedSpawnPoint;
                GameManager.instance.isRedTeam = true;
            }
            else
            {
                plObj.gameObject.transform.position = BlueSpawnPoint;
                GameManager.instance.isRedTeam = false;
            }

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
            plStat = plObj.gameObject.GetComponent<Stat>();
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

        GameUIManager.instance.UpdateMainBar(true);
        GameUIManager.instance.UpdatePlayerStatus(true);

        if (player == NetworkManager.Instance.runner.LocalPlayer)
        {
            plStat.SendInitInfos();
            GameManager.instance.ClientPlayer = plObj.gameObject;
        }
    }
}