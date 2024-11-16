using Fusion;
using Fusion.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    //네트워크 매니저는 주로 Fusion Runner(Core system)를 관리하는 역할을 합니다.
    //모르는 사항은 PM에게 문의해주세요.

    public NetworkRunner RunnerPrefab;
    public NetworkRunner runner;
    private static NetworkManager instance;

    public NetworkObject[] registerList;
    public static NetworkManager Instance //singleton pattern implementation
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        runner = Instantiate(RunnerPrefab);
        var events = runner.GetComponent<NetworkEvents>();
        var sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex));
        startGame(sceneInfo);
    }

    private void Update()
    {
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<NetworkManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "NetworkManager";
            instance = gameObj.AddComponent<NetworkManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    public void startGame(NetworkSceneInfo sceneinfos) {
        {
            var startGameArgs = new StartGameArgs()
            {
                GameMode = Fusion.GameMode.Shared,
                SessionName = PlayerPrefs.GetString("Server"),
                PlayerCount = 6,
                Scene = sceneinfos
                
            };
            runner.StartGame(startGameArgs);

        }
    }
}
