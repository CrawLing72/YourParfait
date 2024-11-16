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
        GameManager instance = FindObjectOfType<GameManager>();
        //Handle TeamThemeBar, productsCount, goodsCount
        if (instance.isRedTeam)
        {
            TeamText.text = "Red Team";
            TeamThemeBar.sprite = redTeamBar;
            ProductsCount.text = string.Format("{0, 3}", instance.RedScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", instance.RedScore_Goods.ToString());
        }
        else
        {
            TeamText.text = "Blue Team";
            TeamThemeBar.sprite = blueTeamBar;
            ProductsCount.text = string.Format("{0, 3}", instance.BlueScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", instance.BlueScore_Goods.ToString());
        }
        KDA.text = "0/0/0"; // -> 나중에 GM에 추가해 놓을것.
    }

    public void UpdatePlayerStatus(bool isSetup = false) //setup과 update를 분리해 놓을것 : 안그러면 IO 배치수 개쪽남
    {

    }

    public void UpdateMainBar(bool isSetup = false)
    {
        if (isSetup)
        {
            Char_Face.sprite = Resources.Load<Sprite>("circled_char_Spoted/" + PlayerPrefs.GetString("CharName"));
            Qskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_Q");
            Eskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_E");
            Wskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_W");
        }
        // item, lv는 차후 수정
    }
}
