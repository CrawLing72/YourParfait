using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager instance;

    [Header ("AudioSources")]
    public AudioSource UI_Source;
    public AudioSource BGM_Source;
    public AudioSource Voice_Source;
    public AudioSource Effect_Source;

    [Header ("AudioClips")]
    public AudioClip[] Clicke_Effects;
    public AudioClip[] BGMS;
    
    public void onClick()
    {
        UI_Source.clip = Clicke_Effects[Random.Range(0, Clicke_Effects.Length)];
        UI_Source.Play();
    }
    
    public void stopPlayingBGM()
    {
        BGM_Source.Stop();
    }

    public void startPlayingBGM()
    {
        BGM_Source.clip = BGMS[Random.Range(0, BGMS.Length)];
        BGM_Source.Play();
    }

    private void Update()
    {
        if (!BGM_Source.isPlaying)
        {
            startPlayingBGM();
        }
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
