using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : NetworkBehaviour, IAttack
{
    [Networked]
    bool isSecondStage { get; set; }

    SpriteRenderer spriteRenderer;

    [Networked]
    int goods_count { get; set; }

    [Networked]
    float totalDamage { get; set; }

    public int maxDamage = 1000;

    public bool isRedTeam = true;
    public int maximumGoodsCount = 20;

    public Slider slider;
    public TMP_Text goodsText;

    private bool isSpawned = false;

    protected void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    public override void Spawned()
    {
        isSecondStage = false;
        goods_count = 0;
        totalDamage = 0f;
    }

    private void Start()
    {
        this.enabled = true;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSecondStage)
        {
            NetworkObject targetObj = collision.gameObject.GetComponent<NetworkObject>();
            GameState gameState = FindObjectOfType<GameState>().GetComponent<GameState>();
            Stat stat = targetObj.gameObject.GetComponent<Stat>();

            if (isRedTeam && stat.isRedTeam)
            {
                if (targetObj.HasStateAuthority) // Master Client인 경우
                {
                    goods_count += stat.goodscount;
                    gameState.RedScore_Goods -= stat.goodscount;
                    
                    stat.goodscount = 0;
                }
                else
                {
                    Rpc_SetGoodsCount(stat.goodscount + goods_count);
                    gameState.RPC_SetRedGoodsCOunt(gameState.RedScore_Goods - stat.goodscount);
                    stat.Rpc_PlusGoodsCount(-stat.goodscount);
                }

            }
            else if (!isRedTeam && !stat.isRedTeam)
            {
                if (targetObj.HasStateAuthority) // Master Client인 경우
                {
                    goods_count += stat.goodscount;
                    gameState.BlueScore_Goods -= stat.goodscount;

                    stat.goodscount = 0;
                }
                else
                {
                    Rpc_SetGoodsCount(stat.goodscount + goods_count);
                    gameState.RPC_SetBlueGoodsCOunt(gameState.RedScore_Goods - stat.goodscount);
                    stat.Rpc_PlusGoodsCount(-stat.goodscount);
                }

            }
        }
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(goods_count >= maximumGoodsCount)
        {
            goods_count = 0;
            isSecondStage = true;
        }
    }

    public void FixedUpdate()
    {
        if (isSpawned)
        {
            if (!isSecondStage) goodsText.text = goods_count.ToString() + "/" + maximumGoodsCount.ToString();
            else
            {
                goodsText.text = "Making..";
                slider.value = totalDamage / maxDamage;

                if (totalDamage >= maxDamage)
                {
                    if (Object.HasStateAuthority)
                    {
                        GameManager.instance.isGameOvered = true;
                        GameManager.instance.isRedTeamWin = isRedTeam;
                    }
                    else
                    {
                        Rpc_gameOver(isRedTeam);
                    }
                }
            }
        }
    }

    public void GetDamage(float Damage)
    {
        totalDamage += Damage;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_PlusDamage(float _damage)
    {
        GetDamage(_damage);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_SetGoodsCount(int _count)
    {
        goods_count = _count;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_gameOver(bool isRedTeam)
    {
        GameManager.instance.isGameOvered = true;
        GameManager.instance.isRedTeamWin = isRedTeam;
    }
}
