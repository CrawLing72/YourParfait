using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using ExitGames.Client.Photon.StructWrapping;

public sealed class GameState : NetworkBehaviour, ISpawned
{
 
    public PlayerSpawner playerSpawner;

    
    // Synced Variables
    [Networked, Capacity(6)] 
    public NetworkArray<int> Players_Char_Index { get; } // -1 : Empty, 0 : Rainyk, etc.
    [Networked, Capacity(6)] 
    public NetworkArray<bool> IsRedTeam_Sync { get; }
    [Networked, Capacity(6)] 
    public NetworkArray<float> HP { get; }
    [Networked, Capacity(6)] 
    public NetworkArray<float> MaxHP { get; }
    [Networked, Capacity(6)] 
    public NetworkArray<float> MP { get; }
    [Networked, Capacity(6)] 
    public NetworkArray<float> MaxMP { get; }
    [Networked] 
    public float GameTime { get; set; }
    [Networked] 
    public int RedScore_Products { get; set; }
    [Networked] 
    public int BlueScore_Products { get; set; }
    [Networked] 
    public int RedScore_Goods { get; set; }
    [Networked] 
    public int BlueScore_Goods { get; set; }
    public int GSSpawned = 0;

    void Start() // -> 최초 들어오는 Client에게만 저장됨.. 이뭔...
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void FixedUpdateNetwork()
    {
        GameTime -= Runner.DeltaTime;
    }

    public override void Spawned()
    {
        // Init. Common Variables
        for (int i = 0; i < 6; i++)
        {
            Players_Char_Index.Set(i, -1);
            IsRedTeam_Sync.Set(i, false);
            HP.Set(i, 0f);
            MaxHP.Set(i, 0f);
            MP.Set(i, 0f);
            MaxMP.Set(i, 0f);
        }
        GameTime = 1800f;
        RedScore_Products = 0;
        BlueScore_Products = 0;
        RedScore_Goods = 0;
        BlueScore_Goods = 0;

        playerSpawner.PlayerJoined(NetworkManager.Instance.runner.LocalPlayer);
        GSSpawned = 1;
    }

    // -> BROADCASTING : Be AWARE of the RPCs
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetProperties(int clinet_index, int char_name, bool _isRedTeam, float _CurrentHP, float _MaxHP, float _CurrentMP, float _MaxMP)
    {
        Players_Char_Index.Set(clinet_index, char_name);
        IsRedTeam_Sync.Set(clinet_index, _isRedTeam);
        HP.Set(clinet_index, _CurrentHP);
        MaxHP.Set(clinet_index, _MaxHP);
        MP.Set(clinet_index, _CurrentMP);
        MaxMP.Set(clinet_index, _MaxMP);
    }

    public void SetProperties(int clinet_index, int char_name, bool _isRedTeam, float _CurrentHP, float _MaxHP, float _CurrentMP, float _MaxMP)
    {
        RPC_SetProperties(clinet_index, char_name, _isRedTeam, _CurrentHP, _MaxHP, _CurrentMP, _MaxMP);
    }
}
