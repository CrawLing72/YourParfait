using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Stat : MonoBehaviour
{
    protected float maxHp, maxMp, currentHp, currentMp, speed, attackRange, attackTime, wTime, eTime, rTime;

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
    public float GetRTime() { return rTime; }
    public void SetTRime(float setValue) { rTime = setValue; }

}
