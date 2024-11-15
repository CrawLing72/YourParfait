using Fusion;
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

    public void PlayerJoined(PlayerRef player)
    {
        if (player == NetworkManager.Instance.runner.LocalPlayer)
        {
            var plObject = NetworkManager.Instance.runner.Spawn(PlayerPrefab, SpawnPoint, Quaternion.identity);
            NetworkManager.Instance.runner.SetPlayerObject(player, plObject);
        }
        foreach (var obj in UICAN) obj.SetActive(true);
        WaitingText.SetActive(false);
    }
}