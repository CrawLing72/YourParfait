using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Table : NetworkBehaviour, IAttack
{
    [Networked]
    float gage { get; set; } = 10.0f;

    [Networked]
    float mwidth { get; set; } = 25.40496f;

    float max = 30.48f;
    float scaley = 1f;
    float scalex = 1f;
    bool bisMax = false;

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
        spriteRenderer.size = new Vector2(mwidth, gage);
        scaley = gameObject.transform.localScale.y;
        scalex = gameObject.transform.localScale.x;


    }

    private void Start()
    {
        max = max * scaley;
        mwidth = mwidth * scalex;
        gage = gage * scaley;

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
        if(goods_count >= maximumGoodsCount)
        {
            goods_count = 0;
            isSecondStage = true;
        }

        slider.value = gage/ max;
        goodsText.text = goods_count.ToString() + "/" + maximumGoodsCount.ToString();
    }

    public void GetDamage(float Damage)
    {
        if (!bisMax && isSecondStage)
        {
            gage += 0.5f * scaley;
            if (gage >= max) // Game Winning Condition.
            {
                GameManager.instance.isGameOvered = true;
                bisMax = true;
            }
            Mathf.Clamp(gage, 0, max);
            spriteRenderer.size = new Vector2(mwidth, gage);
        }
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
