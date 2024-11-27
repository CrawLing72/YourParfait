using AK.Wwise;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void PlaySound(AK.Wwise.Event _event)
    {
        gameObject.AddComponent<AkEvent>();

        _event.Post(gameObject);
        Debug.LogError(_event.Name);
        Invoke("SelfKill", 10f);
    }

    public void SelfKill()
    { 
       Destroy(gameObject);
    }
}
