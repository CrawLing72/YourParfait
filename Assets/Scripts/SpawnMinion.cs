using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField]
    GameObject minionPrefeb;

    [SerializeField]
    float spawnTime;

    bool isSpawn = false;

    private void FixedUpdate()
    {
        if(!isSpawn)
        {
            StartSpawn();
            isSpawn = true;
            Invoke("SetTime", spawnTime);
        }
    }

    void StartSpawn()
    {
        Spawn();
        Invoke("Spawn", 1);
        Invoke("Spawn", 2);
        Invoke("Spawn", 3);
        Invoke("Spawn", 4);
    }

    void Spawn()
    {
        GameObject newMinion = Instantiate(minionPrefeb);
        newMinion.transform.position = gameObject.transform.position;
    }

    void SetTime()
    {
        isSpawn = false ;
    }
}
