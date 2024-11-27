using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AK.Wwise;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager instance;

    public AK.Wwise.Event BGM_Event;
    public AK.Wwise.Event Click_Event;
    public AK.Wwise.Event Confirm_Event;

    public void onClick(bool isClick)
    {
        if(isClick)
        {
            Click_Event.Post(gameObject);
        }
        else
        {
            Confirm_Event.Post(gameObject);
        }
    }
    
    public void stopPlayingBGM()
    {
        BGM_Event.Stop(gameObject);
    }

    public void startPlayingBGM()
    {
        BGM_Event.Post(gameObject);
    }
    void Awake()
    {
        // SingleTone Pattern Implemention
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        startPlayingBGM();
    }
}
