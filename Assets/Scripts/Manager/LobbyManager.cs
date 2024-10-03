using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviour
{
    public TMP_Text CharName;
    public TMP_Text CharDesc;
    public TMP_Text CharPos;
    public Image CharImg;

    public GameObject CharInfoPanel;
    public GameObject CharPrefab;

    public static LobbyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 존재하면 새로 생성된 객체 파괴
        }

        CharName = GameObject.Find("CharName").GetComponent<TMP_Text>();
        CharDesc = GameObject.Find("CharDescription").GetComponent<TMP_Text>();
        CharPos = GameObject.Find("CharPos").GetComponent<TMP_Text>();
        CharImg = GameObject.Find("SelectedChar").GetComponent<Image>();
        CharInfoPanel = GameObject.Find("Content");
    }

    private void Start()
    {
        if(DataManager.Instance.player_info == null)
        {
            DataManager.Instance.player_info = CSVReader.Read("CharacterInfo");
        }

        for(int i = 0; i < DataManager.Instance.player_info.Count; i++)
        {
            Instantiate(CharPrefab, CharInfoPanel.transform);
        }
        for(int i = 0; i<DataManager.Instance.player_info.Count; i++)
        {
            CharButtonController charButton = CharInfoPanel.transform.GetChild(i).GetComponent<CharButtonController>();
            charButton.charName = DataManager.Instance.player_info[i]["ImgFileName"].ToString();
            charButton.index = i;
            charButton.refresh_button_info();
        }
    }

    public void refresh_char_info(int index)
    {
        CharName.text = DataManager.Instance.player_info[index]["Name"].ToString();
        CharDesc.text = DataManager.Instance.player_info[index]["Description"].ToString();
        CharPos.text = DataManager.Instance.player_info[index]["Position"].ToString();
        CharImg.sprite = Resources.Load<Sprite>("CharImgs/" + DataManager.Instance.player_info[index]["ImgFileName"].ToString());
    }

    public void backToMain()
    {
        PlayerPrefs.SetString("NewScene", "MainMenu");
        SceneManager.LoadScene("LoadingScene");
    }
}
