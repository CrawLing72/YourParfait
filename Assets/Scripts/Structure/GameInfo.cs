using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
   //Matchmaking Settings
    public string ServerName;
    public string PlayerName;
    public string CurrentChar;

    //Other Players
    public Dictionary<string, Playerinfo> Players; // id : char와 같이 작성

    //Game Settings
    public int MaxPlayers = 6;
    public int CurrentPlayers = 0;
    public int GameTime = 1800;
    public bool isRedTeam = true;

    //Game Info
    public int RedScore_Products = 0;    
    public int BlueScore_Products = 0;
    public int RedScore_Goods = 0;
    public int BlueScore_Goods = 0;

    public GameInfo()
    {
        Players = new Dictionary<string, Playerinfo>();
    }

}
