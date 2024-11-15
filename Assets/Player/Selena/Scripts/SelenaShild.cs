using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelenaShild : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IFactoryBuff Factory = collision.gameObject.GetComponent<IFactoryBuff>();
        if(Factory != null )
        {
            Factory.GetBuff(1.2f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IFactoryBuff Factory = collision.gameObject.GetComponent<IFactoryBuff>();
        if (Factory != null)
        {
            Factory.OffBuff();
        }
    }

}
