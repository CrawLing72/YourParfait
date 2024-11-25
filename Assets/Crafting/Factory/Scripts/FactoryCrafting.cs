using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FactoryCrafting : NetworkBehaviour, IFactoryBuff
{
    [SerializeField]
    int currentMinion = 0;

    bool bIsFull = false;
    bool bIsMinion = false;

    bool bIsBuff = false;
    float buffValue;


    [Networked]
    float gage { get; set; }

    float maxGage = 100.0f;

    [Networked]
    int resoucrce { get; set; }

    [Networked]
    int goods { get; set; }

    [SerializeField]
    bool isRedTeam = true;

    public TMP_Text SupplyText;
    public Slider SupplySlider;
    public int requiredSupplyForGoods = 10;
    public TMP_Text GoodsText;

    public override void Spawned()
    {
        gage = 0f;
        resoucrce = 0;
        goods = 0;
    }
    public override void FixedUpdateNetwork()
    {
        GameState gameState = FindObjectOfType<GameState>().gameObject.GetComponent<GameState>();
        if (!bIsBuff)
        {
            gage += currentMinion * NetworkManager.Instance.runner.DeltaTime * 10f;
        }
        else
        {
            gage += currentMinion * NetworkManager.Instance.runner.DeltaTime * 10f * buffValue;
        }

        if (gage >= maxGage)
        {
            gage = 0.0f;
            resoucrce += 1;
            if (isRedTeam) gameState.RedScore_Products += 1;
            else gameState.BlueScore_Products += 1;
        }

        if (resoucrce >= requiredSupplyForGoods)
        {
            goods += 1;
            if (isRedTeam) gameState.RedScore_Goods += 1;
            else gameState.BlueScore_Goods += 1;

            if (isRedTeam) gameState.RedScore_Products -= resoucrce;
            else gameState.BlueScore_Products -= resoucrce;
            resoucrce = 0;
        }

        SupplySlider.value = gage / maxGage;
        SupplyText.text= resoucrce.ToString();
        GoodsText.text = goods.ToString();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameState gameState = FindObjectOfType<GameState>().gameObject.GetComponent<GameState>();
        if (isRedTeam)
        {
            MinionsAIRed minion = collision.gameObject.GetComponent<MinionsAIRed>();
            if (minion != null)
            {
                currentMinion += 1;
            }

            if (collision.gameObject.CompareTag("Player") && (goods > 0))
            {
                Stat player_stat = collision.gameObject.GetComponent<Stat>();
                int player_index = player_stat.clientIndex;
                if (gameState.IsRedTeam_Sync.Get(player_index))
                {
                    if(collision.gameObject.GetComponent<NetworkObject>().HasStateAuthority)
                    {
                        player_stat.goodscount += goods;
                        goods = 0;
                    }
                    else
                    {
                        player_stat.Rpc_PlusGoodsCount(goods);
                        Rpc_SetGoodsCount(0);
                    }
                }
            } 
        }
        else
        {
            MinionsAIBlue minion = collision.gameObject.GetComponent<MinionsAIBlue>();
            if (minion != null) // 미니언이 들어오면 카운트 추가
            {
                currentMinion += 1;
            }

            if (collision.gameObject.CompareTag("Player") && (goods>0)) // 플레이어가 콜리전 안에 들어오면 굿즈 떠넘기기 (자동)
            {
                Stat player_stat = collision.gameObject.GetComponent<Stat>();
                int player_index = player_stat.clientIndex;
                if (!gameState.IsRedTeam_Sync.Get(player_index))
                {
                    if (collision.gameObject.GetComponent<NetworkObject>().HasStateAuthority) // Master Client인 경우
                    {
                        player_stat.goodscount += goods;
                        goods = 0;
                    }
                    else
                    {
                        player_stat.Rpc_PlusGoodsCount(goods);
                        Rpc_SetGoodsCount(0);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isRedTeam)
        {
            MinionsAIRed minion = collision.gameObject.GetComponent<MinionsAIRed>();
            if (minion != null)
            {
                currentMinion -= 1;
            }
        }
        else
        {
            MinionsAIBlue minion = collision.gameObject.GetComponent<MinionsAIBlue>();
            if (minion != null)
            {
                currentMinion -= 1;
            }
        }
    }

    void GetBuff(float value)
    {
        bIsBuff = true;
        buffValue = value;
    }

    void OffBuff()
    {
        bIsBuff = false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_SetGoodsCount(int _count)
    {
        goods = _count;
    }
}
