using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroySelf", 0.5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
