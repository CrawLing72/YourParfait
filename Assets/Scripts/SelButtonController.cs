using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelButtonController : MonoBehaviour
{
    TMP_Text CharName;
    TMP_Text CharDesc;
    TMP_Text CharPos;
    
    public string CharNameString;
    public Image CharImg;

    private void Awake()
    {
        CharName = DataManager.Instance.CharName;
        CharDesc = DataManager.Instance.CharDesc;
        CharPos = DataManager.Instance.CharPos;
        CharImg = DataManager.Instance.CharImg;
    }

    public void onClick()
    {
        PlayerPrefs.SetString("CharName", CharNameString);

        foreach (Dictionary<string, object> dict in DataManager.Instance.player_info)
        {
            if (dict["ImgFileName"].ToString() == CharNameString)
            {
                CharName.text = dict["Name"].ToString();
                CharDesc.text = dict["Description"].ToString();
                CharPos.text = dict["Position"].ToString();
                CharImg.sprite = Resources.Load<Sprite>("CharImgs/" + dict["ImgFileName"].ToString());
            }
        }
    }
}
