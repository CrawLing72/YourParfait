using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelController : MonoBehaviour
{
    public GameObject Content;
    public GameObject CharImgPrefab;
    
    private void Awake()
    {
        PlayerPrefs.SetString("CharName", "selena");
        DataManager.Instance.player_info = CSVReader.Read("CharacterInfo");
        Transform BarTransform = Content.transform;
        
        for(int i = 0; i< DataManager.Instance.player_info.Count; i++)
        {
            Instantiate(CharImgPrefab, Content.transform);
        }

        for(int i = 0; i< BarTransform.childCount; i++)
        {
            Transform childTransform = BarTransform.GetChild(i).transform;
            childTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("circled_char/" + DataManager.Instance.player_info[i]["ImgFileName"].ToString());
            childTransform.GetComponent<SelButtonController>().CharNameString = DataManager.Instance.player_info[i]["ImgFileName"].ToString();
        }

    }

}
