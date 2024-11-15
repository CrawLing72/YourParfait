using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FactoryCrafting : MonoBehaviour, IFactoryBuff
{
    [SerializeField]
    int maxMinion = 5;
    int currentMinion = 0;

    bool bIsFull = false;
    bool bIsMinion = false;

    bool bIsBuff = false;
    float buffValue;


    float gage = 0.0f;
    float maxGage = 100.0f;

    int resoucrce = 0;
    
    public void Update()
    {
        if(bIsMinion)
        {
            if (!bIsBuff)
            {
                gage += currentMinion * Time.deltaTime * 0.001f;
            }
            else
            {
                gage += currentMinion * Time.deltaTime * 0.001f*buffValue;
            }

            if (gage >= maxGage)
            {
                gage = 0.0f;
                resoucrce += 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bIsFull)
        {
            MinionTemp minion = collision.gameObject.GetComponent<MinionTemp>();
            if (minion != null)
            {
                bIsMinion = true;
                currentMinion += 1;

                if (currentMinion == maxMinion)
                {
                    bIsFull = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!bIsMinion)
        {
            MinionTemp minion = collision.gameObject.GetComponent<MinionTemp>();
            if (minion != null)
            {
                currentMinion -= 1;
                bIsFull = false;

                if(currentMinion == 0)
                {
                    bIsMinion = false;
                }
            }
        }
    }

    void GetBuff(float value)
    {
        bIsBuff = true;
        buffValue = value;
    }

    void OffBuff()
    {
        bIsBuff = false;
    }
}
