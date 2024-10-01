using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public List<Dictionary<string, object>> player_info;
    public Image CharImg;
    public TMP_Text CharName;
    public TMP_Text CharDesc;
    public TMP_Text CharPos;

    private static DataManager instance;
    public static DataManager Instance //singleton pattern implementation
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
    }
    private static void SetupInstance()
    {
        instance = FindObjectOfType<DataManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "DataManager";
            instance = gameObj.AddComponent<DataManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    /*---------------------------------Player Info---------------------------------*/

    private void Start()
    {
        player_info = CSVReader.Read("CharacterInfo");
    }
}
