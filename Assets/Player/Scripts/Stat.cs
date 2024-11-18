using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Stat : MonoBehaviour
{
    [SerializeField]
    protected float maxHp, maxMp, currentHp, currentMp, speed, attackRange, attackTime, qTime, wTime, eTime,
        ad;

    public int clientIndex; // 얘는 public으로 해두겠습니다 -> 아니면 정신건강에 해로워짐

    private void FixedUpdate()
    {
        GameState instance = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (instance.GSSpawned == 1)
        {
            // Set Synced Variables
            clientIndex = PlayerPrefs.GetInt("ClientIndex");
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
            if (instance == null) Debug.LogError("instace is null");
            else if (GameManager.instance == null) Debug.LogError("GM is Null");
            else if (GameManager.instance.isRedTeam == null) Debug.LogError("isRedTeam is null");
            else if (currentHp == null) Debug.LogError("currentHp is null");
            else if (maxHp == null) Debug.LogError("maxHp is null");
            else if (currentMp == null) Debug.LogError("currentMp is null");
            else if (maxMp == null) Debug.LogError("maxMp is null");
            instance.RPC_SetProperties(clientIndex, char_name, GameManager.instance.isRedTeam, currentHp, maxHp, currentMp, maxMp, PlayerRef.FromIndex(clientIndex));
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
