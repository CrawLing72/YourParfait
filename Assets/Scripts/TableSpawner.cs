using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSpawner: NetworkBehaviour
{
    [Header("Table Obejct")]
    public GameObject RedTeam;
    public GameObject BlueTeam;

    [Header("Positions")]
    public Transform Spawned_position;

    [Header("Properties")]
    public bool isRedTeam;

    [Networked]
    public bool isSpawned { get; set; } = false;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (!isSpawned && (gameState.GSSpawned == 1))
        {
            NetworkManager.Instance.runner.Spawn(isRedTeam ? RedTeam : BlueTeam, Spawned_position.position, Quaternion.identity, gameState.masterRef);
            isSpawned = true;
        }
    }

}
