using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharButtonController : MonoBehaviour
{
    public string charName;
    public int index;
    
    public void refresh_button_info()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("circled_char/" + charName);
        PlayerPrefs.SetString("SelectedChar", charName);
    }

    public void OnClick()
    {
        LobbyManager.instance.refresh_char_info(index);
    }
}
