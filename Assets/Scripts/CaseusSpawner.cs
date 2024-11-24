using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseusSpawner : NetworkBehaviour
{
    [Header("Minion Obejct")]
    public GameObject RedTeamCaseus;
    public GameObject BlueTeamCaseus;

    [Header("Settigns")]
    public bool isRedTeam = true;
    public int maximumMinion = 10;
    public float ReSpawnTime = 5f;

    [Header("Points")]
    public GameObject RedSpawnPoint;
    public GameObject BlueSpawnPoint;
    public GameObject RedFactoryPoint1;
    public GameObject RedFactoryPoint2;
    public GameObject BlueFactoryPoint1;
    public GameObject BlueFactoryPoint2;
    public GameObject CraftingTable;

    private float timer = 0f;
    private MinionsAIBlue MinionsAIBlue;
    private MinionsAIRed MinionsAIRed;

    [Networked]
    private bool isUpper { get; set; } = false;

    public override void FixedUpdateNetwork()
    {
        if (GameManager.instance.isGameStarted)
        {
            GameState gameState = FindObjectOfType<GameState>().gameObject.GetComponent<GameState>();

            timer += NetworkManager.Instance.runner.DeltaTime;
            int current_minion = isRedTeam ? gameState.RedScore_Minions : gameState.BlueScore_Minions;
            if ((timer > ReSpawnTime) && (current_minion < maximumMinion))
            {
                NetworkObject Spawned_Caseus = NetworkManager.Instance.runner.Spawn(isRedTeam ? RedTeamCaseus : BlueTeamCaseus, transform.position, Quaternion.identity, gameState.masterRef);
                timer = 0f;
                if (isRedTeam)
                {
                    gameState.RedScore_Minions += 1;
                    MinionsAIRed = Spawned_Caseus.gameObject.GetComponent<MinionsAIRed>();
                    MinionsAIRed.Spawner = RedSpawnPoint.transform;

                    if (isUpper)
                    {
                        MinionsAIRed.Resource = RedFactoryPoint1.transform;
                        isUpper = false;
                    }
                    else
                    {
                        MinionsAIRed.Resource = RedFactoryPoint2.transform;
                        isUpper = true;
                    }
                }
                else
                {
                    gameState.BlueScore_Minions += 1;
                    MinionsAIBlue = Spawned_Caseus.gameObject.GetComponent<MinionsAIBlue>();
                    MinionsAIBlue.Spawner = BlueSpawnPoint.transform;

                    if (isUpper)
                    {
                        MinionsAIBlue.Resource = BlueFactoryPoint1.transform;
                        isUpper = false;
                    }
                    else
                    {
                        MinionsAIBlue.Resource = BlueFactoryPoint2.transform;
                        isUpper = true;
                    }
                }

            }
        }
    }

    public override void Spawned()
    {
        base.Spawned();
    }
}
