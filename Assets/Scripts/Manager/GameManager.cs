using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    public bool isRedTeam = true;
    public PlayerSpawner playerSpawner;
    // Synced Variables
    [Networked, Capacity(6)] public NetworkArray<int> Players_Char_Index { get; } // -1 : Empty, 0 : Rainyk, etc.
    [Networked, Capacity(6)] public NetworkArray<bool> IsRedTeam_Sync { get; }
    [Networked, Capacity(6)] public NetworkArray<float> HP { get; }
    [Networked, Capacity(6)] public NetworkArray<float> MaxHP { get; }
    [Networked, Capacity(6)] public NetworkArray<float> MP { get; }
    [Networked, Capacity(6)] public NetworkArray<float> MaxMP { get; }
    [Networked] public float GameTime { get; set; }
    [Networked] public int RedScore_Products { get; set; }
    [Networked] public int BlueScore_Products { get; set; }
    [Networked] public int RedScore_Goods { get; set; }
    [Networked] public int BlueScore_Goods { get; set; }

    public bool isSpanwed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        GameUIManager.instance.UpdateMainBar(true);
        PlayerPrefs.SetInt("ClientIndex", NetworkManager.Instance.runner.SessionInfo.PlayerCount - 1);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (Object.HasStateAuthority)
        {
            GameTime -= Runner.DeltaTime;
        }

        GameUIManager.instance.UpdateTopStatusBar();
        GameUIManager.instance.UpdateMainBar();
        GameUIManager.instance.UpdatePlayerStatus();

        foreach (PlayerRef player in NetworkManager.Instance.runner.ActivePlayers)
        {
            NetworkManager.Instance.runner.TryGetPlayerObject(player, out NetworkObject playerObj);
            if (playerObj)
            {
                Debug.Log(playerObj.GetComponent<Stat>().name);
            }
            else
            {
                Debug.LogWarning("Player Object is null");
            }
        }
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            // Init. Common Variables
            for (int i = 0; i < 6; i++)
            {
                Players_Char_Index.Set(i, -1);
                IsRedTeam_Sync.Set(i, false);
                HP.Set(i, 0);
                MaxHP.Set(i, 0);
                MP.Set(i, 0);
                MaxMP.Set(i, 0);
            }
            GameTime = 1800f;
            RedScore_Products = 0;
            BlueScore_Products = 0;
            RedScore_Goods = 0;
            BlueScore_Goods = 0;
        }
        playerSpawner.SettingInfos();
    }
}
