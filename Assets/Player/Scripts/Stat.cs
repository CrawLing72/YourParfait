using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Stat : MonoBehaviour
{
    protected float maxHp, maxMp, currentHp, currentMp, speed, attackRange;

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

}
