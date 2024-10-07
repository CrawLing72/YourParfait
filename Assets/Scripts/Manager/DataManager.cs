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
    public List<Dictionary<string, object>> Character_info;
    public bool isLogined;
    public string JWTToken;

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
        Character_info = CSVReader.Read("CharacterInfo");
    }
}
