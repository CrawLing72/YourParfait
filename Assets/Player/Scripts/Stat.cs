using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using TMPro;
public class Stat : NetworkBehaviour
{
    [SerializeField]
    protected float maxHp, maxMp, currentHp, currentMp, speed, attackRange, attackTime, qTime, wTime, eTime,
        ad;

    [Networked]
    public int goodscount { get; set; }


    public int clientIndex; // 얘는 public으로 해두겠습니다 -> 아니면 정신건강에 해로워짐
    public bool isRedTeam = true;
    public GameObject Canvus;
    public TMP_Text GoodsCount; // 얜 표시 Text임

    GameState instance;

    // ---- Init Char ----- ///
    private async Task WaitForGSSpawned(GameState instance)
    {
        while (instance.GSSpawned != 1)
        {
            Debug.Log("Waiting for GSSpawned to be 1...");
            await Task.Yield(); // 다음 프레임으로 넘김
        }
    }

    private void InitializeClient(GameState instance)
    {
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

        instance.RPC_SetChar(clientIndex, char_name);
        instance.RPC_SetTeam(clientIndex, GameManager.instance.isRedTeam);
        instance.RPC_SetHP(clientIndex, currentHp, maxHp);
        instance.RPC_SetMP(clientIndex, currentMp, maxMp);

        goodscount = 0;

        Debug.Log("Initialization complete!");

        isRedTeam = instance.IsRedTeam_Sync.Get(clientIndex);
    }
    public async void SendInitInfos()
    {
        instance = FindObjectOfType<GameState>().GetComponent<GameState>();

        // GSSpawned가 1이 될 때까지 대기
        await WaitForGSSpawned(instance);

        // 초기화 로직 실행
        InitializeClient(instance);
    }

    public override void FixedUpdateNetwork()
    {
        if(goodscount > 0)
        {
            Canvus.SetActive(true);
            GoodsCount.text = goodscount.ToString();
        }
        else
        {
            Canvus.SetActive(false);
        }
    }

    // -- end -- //
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_PlusGoodsCount(int _count)
    {
       goodscount += _count;
    }

    public void SetGoodsCount(int _count)
    {
        goodscount = _count;
    }

    public float GetMaxHp() { return maxHp; }
    public void SetMaxHp(float setValue){ maxHp = setValue; }
    public float GetCurrentHp() { return currentHp; }
    public void SetCurrentHp(float setValue) 
    { 
        Mathf.Clamp(setValue, 0, maxHp);
        currentHp = setValue; 
        Debug.LogError("SetCurrentHp: " + currentHp);
        Debug.LogError("SetCurrentmaxHp: " + maxHp);
        Debug.LogError("SetCurrentIndex: " + clientIndex);
        instance.RPC_SetHP(clientIndex, currentHp, maxHp);

    }
    public float GetMaxMp() { return maxMp; }
    public void SetMaxMp(float setValue) { maxMp = setValue; }
    public float GetCurrentMp() { return currentMp; }
    public void SetCurrentMp(float setValue) 
    {
        Mathf.Clamp(setValue, 0, maxMp);
        currentMp = setValue; 
        instance.RPC_SetMP(clientIndex, currentMp, maxMp);
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
