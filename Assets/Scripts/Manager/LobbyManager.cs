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
    private string CharFileName;

    public GameObject CharInfoPanel;
    public GameObject CharPrefab;

    public static LobbyManager instance;

    public class MatchedServer
    {
        public string arranged_server;
        public int player_count;
    }

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
        if(DataManager.Instance.Character_info == null)
        {
            DataManager.Instance.Character_info = CSVReader.Read("CharacterInfo");
        }

        for(int i = 0; i < DataManager.Instance.Character_info.Count; i++)
        {
            Instantiate(CharPrefab, CharInfoPanel.transform);
        }
        for(int i = 0; i<DataManager.Instance.Character_info.Count; i++)
        {
            CharButtonController charButton = CharInfoPanel.transform.GetChild(i).GetComponent<CharButtonController>();
            charButton.charName = DataManager.Instance.Character_info[i]["ImgFileName"].ToString();
            charButton.index = i;
            charButton.refresh_button_info();
        }
    }

    public void refresh_char_info(int index)
    {
        CharName.text = DataManager.Instance.Character_info[index]["Name"].ToString();
        CharDesc.text = DataManager.Instance.Character_info[index]["Description"].ToString();
        CharPos.text = DataManager.Instance.Character_info[index]["Position"].ToString();
        CharImg.sprite = Resources.Load<Sprite>("CharImgs/" + DataManager.Instance.Character_info[index]["ImgFileName"].ToString());
        CharFileName = DataManager.Instance.Character_info[index]["ImgFileName"].ToString();
    }

    public void backToMain()
    {
        PlayerPrefs.SetString("NewScene", "MainMenu");
        SceneManager.LoadScene("LoadingScene");
    }

    public async void startGame()
    {
        await APIManager.instance.sendJsonData(PlayerPrefs.GetString("Name"), PlayerPrefs.GetString("Password"), "matchmaking");
        long response_code = APIManager.instance.answered_data.response_code;
        if (response_code == 200)
        {
            MatchedServer matchedServer = JsonUtility.FromJson<MatchedServer>(APIManager.instance.answered_data.message);
            PlayerPrefs.SetString("Server", matchedServer.arranged_server);
            PlayerPrefs.SetInt("PlayerCount", matchedServer.player_count);
            Debug.Log("Matched Server : " + matchedServer.arranged_server);
            PlayerPrefs.SetString("NewScene", "GameScene");
            PlayerPrefs.SetString("CharName", CharFileName);
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("Error : " + response_code);
        }
    }
}
