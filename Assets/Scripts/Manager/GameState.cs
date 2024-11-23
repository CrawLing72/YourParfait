using UnityEngine;
using Fusion;
using static Unity.Collections.Unicode;

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

    public override void FixedUpdateNetwork() // GameState Sync는 첫 클라가 정보 관리 담당, 나머지가 구독. 필요시 RPC로 변경.
    {
        GameTime -= Runner.DeltaTime;
        if(PlayerPrefs.GetInt("ClientIndex") == 0) // Set First Client as Info Admin.
        {
            for(int i = 0; i < 6; i++) // Send infos to Cleints Except Admin.
            {
                RPC_SetProperties(i, Players_Char_Index.Get(i), IsRedTeam_Sync.Get(i), HP.Get(i), MaxHP.Get(i), MP.Get(i), MaxMP.Get(i));
            }

        }
    }

    public override void Spawned()
    {
        PlayerPrefs.SetInt("ClientIndex", NetworkManager.Instance.runner.SessionInfo.PlayerCount - 1);
        Debug.LogError("Client Index : " + PlayerPrefs.GetInt("ClientIndex"));

        if (PlayerPrefs.GetInt("ClientIdex") == 0)
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
        }
        playerSpawner.PlayerJoined(NetworkManager.Instance.runner.LocalPlayer);
        GSSpawned = 1;
    }

    // -> BROADCASTING : Admin Client가 각각의 모든 Client에게 정보를 뿌림. 단, Not admin은 구독만 가능
    // ->> 값 변경시 Admin에 RPC 날릴것
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetProperties(int clinet_index, int char_name, bool _isRedTeam, float _CurrentHP, float _MaxHP, float _CurrentMP, float _MaxMP)
    {
        Players_Char_Index.Set(clinet_index, char_name);
        IsRedTeam_Sync.Set(clinet_index, _isRedTeam);
        HP.Set(clinet_index, _CurrentHP);
        MaxHP.Set(clinet_index, _MaxHP);
        MP.Set(clinet_index, _CurrentMP);
        MaxMP.Set(clinet_index, _MaxMP);
    }

    // UNDER : Admin이 아닌 Client가 Admin Client에 정보를 뿌림. 각 Property에 맞게 설정함.
    [Rpc (RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetHP(int _clinet_index, float _HP, float _MaxHP)
    {
        HP.Set(_clinet_index, _HP);
        MaxHP.Set(_clinet_index, _MaxHP);
    }

    [Rpc (RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetMP(int _clinet_index, float _MP, float _MaxMP)
    {
        MP.Set(_clinet_index, _MP);
        MaxMP.Set(_clinet_index, _MaxMP);
    }

    [Rpc (RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetTeam(int _clinet_index, bool _isRedTeam)
    {
        IsRedTeam_Sync.Set(_clinet_index, _isRedTeam);
    }

    [Rpc (RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetChar(int _clinet_index, int _char_name)
    {
        Players_Char_Index.Set(_clinet_index, _char_name);
    }
}
