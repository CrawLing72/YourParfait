using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelenaShild : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IFactoryBuff Factory = collision.gameObject.GetComponent<IFactoryBuff>();
        if (Factory != null)
        {
            Factory.GetBuff(1.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IFactoryBuff Factory = collision.gameObject.GetComponent<IFactoryBuff>();
        if (Factory != null)
        {
            Factory.OffBuff();
        }
    }

}
