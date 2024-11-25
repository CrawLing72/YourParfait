using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : NetworkBehaviour, IAttack
{
    [Networked]
    bool isSecondStage { get; set; } = false;

    SpriteRenderer spriteRenderer;

    [Networked]
    int goods_count { get; set; } = 0;

    public bool isRedTeam = true;
    public int maximumGoodsCount = 20;

    public Slider slider;
    public TMP_Text goodsText;

    protected void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    private void Start()
    {
        this.enabled = true;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
        goodsText.text = goods_count.ToString() + "/" + maximumGoodsCount.ToString();
    }

    public void GetDamage(float Damage)
    {

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_SetDamage(float _damage)
    {
        GetDamage(_damage);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_SetGoodsCount(int _count)
    {
        goods_count = _count;
    }
}
