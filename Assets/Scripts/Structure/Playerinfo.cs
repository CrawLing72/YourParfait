using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinfo
{
    public string id;
    public bool isRedTeam;
    public float HP;
    public float maxHP;
    public float MP;
    public float maxMP;
    public string currentChar;

    public void SetParameters(string _id, bool _isRedTeam, float _HP, float _maxHP, float _MP, float _maxMP, string _CChar)
    {
        id = _id;
        isRedTeam = _isRedTeam;
        HP = _HP;
        MP = _MP;
        maxHP = _maxHP;
        maxMP = _maxMP;
        currentChar = _CChar;
    }
}
