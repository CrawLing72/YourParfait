using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IAttack
{
    float gage = 10.0f;
    float max = 30.48f;
    float mwidth = 25.40496f;
    float scaley = 1.0f;
    float scalex = 1.0f;

    bool bisMax = false;

    SpriteRenderer spriteRenderer;

    protected void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(mwidth, gage);
        //scaley = gameObject.transform.localScale.y;
        //scalex = gameObject.transform.localScale.x;


    }

    private void Start()
    {
        max = max * scaley;
        mwidth = mwidth * scalex;
        gage = gage * scaley;

    }

    protected void Update()
    {
        
    }

    public void GetDamage(float Damage)
    {
        Debug.Log("Table Damage");

        if (!bisMax)
        {
            gage += 0.5f * scaley;
            if (gage >= max)
            {
                bisMax = true;
            }
            Mathf.Clamp(gage, 0, max);
            spriteRenderer.size = new Vector2(mwidth, gage);
        }
    }
}
