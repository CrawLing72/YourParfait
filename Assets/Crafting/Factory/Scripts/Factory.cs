using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Factory : MonoBehaviour, IAttack
{
    float gage = 5.0f;
    float max = 13.23f;

    SpriteRenderer spriteRenderer;

    protected void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(4.77f, gage);
    }

    protected void Update()
    {
    }
    public virtual void GetDamage(float Damage)
    {
        gage += 0.5f;
        Mathf.Clamp(gage, 0, max);
        spriteRenderer.size = new Vector2(4.77f, gage);
    }


}
