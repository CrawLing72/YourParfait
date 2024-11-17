using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void GetDamage(float Damage) { }

    void GetSilent(float time) { }

    void GetSlow(float value, float time) { }

    void Getbondage(float time) { }
}
