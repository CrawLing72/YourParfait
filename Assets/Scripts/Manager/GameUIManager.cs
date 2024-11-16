using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{

    public static GameUIManager instance;

    [Header("TopStatusBar")]
    public Image TeamThemeBar;
    public TMP_Text ProductsCount;
    public TMP_Text GoodsCount;
    public TMP_Text KDA;
    public TMP_Text TeamText;

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

    private Sprite blueTeamBar;
    private Sprite redTeamBar;
    private TMP_Text HP_Text;
    private TMP_Text MP_Text;
    private void Awake()
    {
        //singleton pattern implementation
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        blueTeamBar = Resources.Load<Sprite>("UI/BlueTeam_Bar");
        redTeamBar = Resources.Load<Sprite>("UI/RedTeam_Bar");
        HP_Text = HP_Bar.transform.GetChild(2).GetComponent<TMP_Text>();
        MP_Text = MP_Bar.transform.GetChild(2).GetComponent<TMP_Text>();
    }
    public void UpdateTopStatusBar()
    {
        //Handle TeamThemeBar, productsCount, goodsCount
        if (GameManager.instance.gameInfo.isRedTeam)
        {
            TeamText.text = "Red Team";
            TeamThemeBar.sprite = redTeamBar;
            ProductsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.RedScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.RedScore_Goods.ToString());
        }
        else
        {
            TeamText.text = "Blue Team";
            TeamThemeBar.sprite = blueTeamBar;
            ProductsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.BlueScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", GameManager.instance.gameInfo.BlueScore_Goods.ToString());
        }
        KDA.text = "0/0/0"; // -> 나중에 GM에 추가해 놓을것.
    }

    public void UpdatePlayerStatus(bool isSetup = false) //setup과 update를 분리해 놓을것 : 안그러면 IO 배치수 개쪽남
    {
        Dictionary<string, Sprite> stats = new Dictionary<string, Sprite>();
        if (isSetup)
        {
            foreach (KeyValuePair<string, Playerinfo> objs in GameManager.instance.gameInfo.Players)
            {
                if (objs.Value.isRedTeam && GameManager.instance.gameInfo.isRedTeam)
                    stats[objs.Value.id] = Resources.Load<Sprite>("circled_char/" + objs.Value.currentChar);
                else if (!objs.Value.isRedTeam && !GameManager.instance.gameInfo.isRedTeam)
                    stats[objs.Value.id] = Resources.Load<Sprite>("circled_char/" + objs.Value.currentChar);
            }
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

    public void UpdateMainBar(bool isSetup = false)
    {
        if (isSetup)
        {
            Char_Face.sprite = Resources.Load<Sprite>("circled_char_Spoted/" + GameManager.instance.gameInfo.CurrentChar);
            Qskill.sprite = Resources.Load<Sprite>("UI/Skill/" + GameManager.instance.gameInfo.CurrentChar + "_Q");
            Eskill.sprite = Resources.Load<Sprite>("UI/Skill/" + GameManager.instance.gameInfo.CurrentChar + "_E");
            Wskill.sprite = Resources.Load<Sprite>("UI/Skill/" + GameManager.instance.gameInfo.CurrentChar + "_W");
        }

        float hp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].HP;
        float maxhp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].maxHP;
        HP_Bar.value = hp / maxhp;
        HP_Text.text = hp.ToString() + "/" + maxhp.ToString();

        float mp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].MP;
        float maxmp = GameManager.instance.gameInfo.Players[GameManager.instance.gameInfo.PlayerName].maxMP;
        MP_Bar.value = mp / maxmp;
        MP_Text.text = mp.ToString() + "/" + maxmp.ToString();

        // item, lv는 차후 수정
    }

    
}
