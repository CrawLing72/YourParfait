using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    //��Ʈ��ũ �Ŵ����� �ַ� Fusion Runner(Core system)�� �����ϴ� ������ �մϴ�.
    //�𸣴� ������ PM���� �������ּ���.

    public NetworkRunner runner;

    private static NetworkManager instance;
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

        startGame();
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

    public void startGame() {
        {
            var startGameArgs = new StartGameArgs()
            {
                GameMode = Fusion.GameMode.Shared,
                SessionName = PlayerPrefs.GetString("Server"),
                PlayerCount = 6 // DO NOT CHANGE!!!!!!!
            };

            runner.StartGame(startGameArgs);

        }
    }
}
