using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public GameObject SpawnPoint;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            Runner.Spawn(PlayerPrefab, SpawnPoint.transform.position, Quaternion.identity);
        }
    }
}