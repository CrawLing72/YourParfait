using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;

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
    public Image HP_Bar;
    public Image MP_Bar;
    public Image Qskill;
    public Image Wskill;
    public Image Eskill;
    public GameObject ItemContent;
    public TMP_Text LVText;

    private Sprite blueTeamBar;
    private Sprite redTeamBar;
    private TMP_Text HP_Text;
    private TMP_Text MP_Text;

    GameState gmInstance;
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
        HP_Text = HP_Bar.transform.GetChild(0).GetComponent<TMP_Text>();
        MP_Text = MP_Bar.transform.GetChild(0).GetComponent<TMP_Text>();
    }
    public void UpdateTopStatusBar()
    {
        gmInstance = FindObjectOfType<GameState>().GetComponent<GameState>();
        //Handle TeamThemeBar, productsCount, goodsCount
        if (GameManager.instance.isRedTeam)
        {
            TeamText.text = "Red Team";
            TeamThemeBar.sprite = redTeamBar;
            ProductsCount.text = string.Format("{0, 3}", gmInstance.RedScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", gmInstance.RedScore_Goods.ToString());
        }
        else
        {
            TeamText.text = "Blue Team";
            TeamThemeBar.sprite = blueTeamBar;
            ProductsCount.text = string.Format("{0, 3}", gmInstance.BlueScore_Products.ToString());
            GoodsCount.text = string.Format("{0, 3}", gmInstance.BlueScore_Goods.ToString());
        }
        KDA.text = "0/0/0."; // -> 나중에 GM에 추가해 놓을것.
    }

    public void UpdatePlayerStatus(bool isSetup = false) //setup과 update를 분리해 놓을것 : 안그러면 IO 배치수 개쪽남
    {
        gmInstance = FindObjectOfType<GameState>().GetComponent<GameState>();
        List<int> char_index = new List<int>();
        if (isSetup)
        {
            for (int i = 0; i < 6; i++)
            {
                if(gmInstance.IsRedTeam_Sync.Get(i)) char_index.Add(i);
            }
            int count = 0;
            foreach(int idx in char_index)
            {
                string player_name;
                switch (gmInstance.Players_Char_Index[idx])
                {
                    case 0: player_name = "Rainyk"; break;
                    case 1: player_name = "Selena"; break;
                    case 2: player_name = "Seraphina"; break;
                    case 3: player_name = "Mixube"; break;
                    case 4: player_name = "Tyneya"; break;
                    default: player_name = "Selena"; break;
                }
                Player_stats[count].GetComponent<Image>().sprite = Resources.Load<Sprite>("circled_char_Spoted/"+player_name );
                Player_stats[count].transform.GetChild(0).GetComponent<Slider>().value = (gmInstance.HP.Get(idx) / gmInstance.MaxHP.Get(idx));
                count++;
            }
        }
    }

    public void UpdateMainBar(bool isSetup = false)
    {
        int clinet_index = PlayerPrefs.GetInt("ClientIndex");
        gmInstance = FindObjectOfType<GameState>().GetComponent<GameState>();
        if (isSetup)
        {
            Char_Face.sprite = Resources.Load<Sprite>("circled_char_Spoted/" + PlayerPrefs.GetString("CharName"));
            Qskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_Q");
            Eskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_E");
            Wskill.sprite = Resources.Load<Sprite>("UI/Skill/" + PlayerPrefs.GetString("CharName") + "_W");
        }

        var HPValue = gmInstance.HP.Get(clinet_index) / gmInstance.MaxHP.Get(clinet_index);
        var MPValue = gmInstance.MP.Get(clinet_index) / gmInstance.MaxMP.Get(clinet_index);

        Debug.LogError("HPValue : " + HPValue + " MPValue : " + MPValue);

        HP_Bar.fillAmount = (float)HPValue;
        MP_Bar.fillAmount = (float)MPValue;

        HP_Text.text = gmInstance.HP.Get(clinet_index).ToString() + " / " + gmInstance.MaxHP.Get(clinet_index).ToString();
        MP_Text.text = gmInstance.MP.Get(clinet_index).ToString() + " / " + gmInstance.MaxMP.Get(clinet_index).ToString();
        // item, lv는 차후 수정
    }
}
