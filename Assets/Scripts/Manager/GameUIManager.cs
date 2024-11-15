using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("TopStatusBar")]
    public Image TeamThemeBar;
    public TMP_Text ProductsCount;
    public TMP_Text GoodsCount;
    public TMP_Text KDA;

    [Header("Player_Status")]
    public GameObject RootObj;
    public GameObject[] Player_stats;

    [Header("MainBar")]
    public Image Char_Face;
    public Slider HP_Bar;
    public Slider MP_Bar;
    public Image Qskill;
    public Image Wskill;
    public Image Eskill;
    public GameObject ItemContent;
    public TMP_Text LVText;

    public void UpdateTopStatusBar()
    {
        //Handle TeamThemeBar, productsCount, goodsCount
        if (GameManager.instance.gameInfo.isRedTeam)
        {
            TeamThemeBar.sprite = Resources.Load<Sprite>("/UI/RedTeam_Bar");
            ProductsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.RedScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.RedScore_Goods.ToString());
        }
        else
        {
            TeamThemeBar.sprite = Resources.Load<Sprite>("UI/BlueTeam_Bar");
            ProductsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.BlueScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.BlueScore_Goods.ToString());
        }
        KDA.text = "0/0/0"; // -> 나중에 GM에 추가해 놓을것.
    }

    public void UpdatePlayerStatus() //setup과 update를 분리해야 하나 "잘 돌아가면 그만"입니다 - 신희동
    {
        Dictionary<string, Sprite> stats = new Dictionary<string, Sprite>();
        foreach (KeyValuePair<string, Playerinfo> objs in GameManager.instance.gameInfo.Players)
        {
            if (objs.Value.isRedTeam && GameManager.instance.gameInfo.isRedTeam)
                stats[objs.Value.id] = Resources.Load<Sprite>("circled_char/" + objs.Value.currentChar);
            else if (!objs.Value.isRedTeam && !GameManager.instance.gameInfo.isRedTeam)
                stats[objs.Value.id] = Resources.Load<Sprite>("circled_char/" + objs.Value.currentChar);
        }

        int temp_count = 0;
        foreach(KeyValuePair<string, Sprite> temp in stats)
        {
            Player_stats[temp_count].GetComponent<Image>().sprite = temp.Value;
            Player_stats[temp_count].transform.GetChild(0).GetComponent<Slider>().value
                = GameManager.instance.gameInfo.Players[temp.Key].HP / GameManager.instance.gameInfo.Players[temp.Key].maxHP;
            temp_count += 1;
        }
    }

    public void UpdateMainBar()
    {
        Char_Face.sprite = Resources.Load<Sprite>("circled_char_Spoted/" + GameManager.instance.gameInfo.CurrentChar);

        float hp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].HP;
        float maxhp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].maxHP;
        HP_Bar.value = hp / maxhp;
        TMP_Text HPText = HP_Bar.transform.GetChild(0).GetComponent<TMP_Text>();
        HPText.text = hp.ToString() + "/" + maxhp.ToString();

        float mp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].MP;
        float maxmp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].maxMP;
        MP_Bar.value = mp / maxmp;
        TMP_Text MPText = MP_Bar.transform.GetChild(0).GetComponent<TMP_Text>();
        MPText.text = mp.ToString() + "/" + maxmp.ToString();

        // skill, item, lv는 차후 수정
    }

    
}
