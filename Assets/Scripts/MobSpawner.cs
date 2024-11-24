using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : NetworkBehaviour
{
    [Header("Mob Obejct")]
    public GameObject Mola;
    public GameObject Tree;

    [Header("Settigns")]
    public bool isMola = true;
    [Networked]
    public float respawnTime { get; set; } = 5f;

    private NetworkObject CurrentMob;
    private float timer = 0f;

    public override void FixedUpdateNetwork()
    {
        if(CurrentMob == null) timer += NetworkManager.Instance.runner.DeltaTime;
        if (GameManager.instance.isGameStarted)
        {
            GameState gameState = FindObjectOfType<GameState>().gameObject.GetComponent<GameState>();

            if ((CurrentMob == null) && (timer > respawnTime))
            {
                NetworkObject Spawned_Mob = NetworkManager.Instance.runner.Spawn(isMola ? Mola : Tree, transform.position, Quaternion.identity, gameState.masterRef);
                CurrentMob = Spawned_Mob;
                timer = 0f;
            }
        }
    }
}

