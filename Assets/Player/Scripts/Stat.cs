using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected float maxHp, maxMp, currentHp, currentMp, speed, attackRange, attackTime, qTime, wTime, eTime,
        ad;

    private void FixedUpdate()
    {
        GameState instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (instance.GSSpawned == 1)
        {
            // Set Synced Variables
            int clientIndex = PlayerPrefs.GetInt("ClientIndex");
            int char_name;
            switch (PlayerPrefs.GetString("CharName"))
            {
                case "Selena": char_name = 1; break;
                case "Seraphina": char_name = 2; break;
                case "Mixube": char_name = 3; break;
                case "Tyneya": char_name = 4; break;
                case "Rainyk": char_name = 0; break;
                default: char_name = 1; break;
            }
            instance.SetProperties(clientIndex, char_name, GameManager.instance.isRedTeam, currentHp, maxHp, currentMp, maxMp);
        }
    }

    public float GetMaxHp() { return maxHp; }
    public void SetMaxHp(float setValue){ maxHp = setValue; }
    public float GetCurrentHp() { return currentHp; }
    public void SetCurrentHp(float setValue) 
    { 
        Mathf.Clamp(setValue, 0, maxHp);
        currentHp = setValue; 

    }
    public float GetMaxMp() { return maxMp; }
    public void SetMaxMp(float setValue) { maxMp = setValue; }
    public float GetCurrentMp() { return currentMp; }
    public void SetCurrentMp(float setValue) 
    {
        Mathf.Clamp(setValue, 0, maxMp);
        currentMp = setValue; 
    }
    public float GetSpeed() { return speed; }
    public void SetSpeed(float setValue) { speed = setValue; }
    public float GetAttackRange() { return attackRange; }
    public void SetAttackRange(float setValue) { attackRange = setValue; }
    public float GetAttackTime() { return attackTime; }
    public void SetAttackTime(float setValue) { attackTime = setValue; }
    public float GetWTime() { return wTime; }
    public void SetTWime(float setValue) { wTime = setValue; }
    public float GetETime() { return eTime; }
    public void SetTEime(float setValue) { eTime = setValue; }
    public float GetQTime() { return qTime; }
    public void SetQTime(float setValue) { qTime = setValue; }
    public float GetAd() { return ad; }
    public void SetAd(float setValue) { ad = setValue; }

}
