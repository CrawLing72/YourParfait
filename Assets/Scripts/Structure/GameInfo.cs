using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
   //Matchmaking Settings
    public string ServerName;
    public string PlayerName;

    //Other Players
    public Dictionary<string, string> Players = new Dictionary<string, string>(); // id : char와 같이 작성
    public Dictionary<string, string> PlayersChar = new Dictionary<string, string>(); // id : team과 같이 작성
    public Dictionary<string, int> PlayersHP = new Dictionary<string, int>(); // id : hp과 같이 작성

    //Game Settings
    public int MaxPlayers = 6;
    public int GameTime = 1800;
    public bool isRedTeam = true;

    //Game Info
    public int RedScore_Products = 0;    
    public int BlueScore_Products = 0;
    public int RedScore_Goods = 0;
    public int BlueScore_Goods = 0;

}
