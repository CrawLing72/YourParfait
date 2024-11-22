using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EffectTimer : NetworkBehaviour
{
    [SerializeField]
    float time;
    // Start is called before the first frame update

    public override void FixedUpdateNetwork()
    {
        time -= NetworkManager.Instance.runner.DeltaTime;
        if (time <= 0)
        {
            Despawn();
        }
    }

    void Despawn()
    {
        NetworkManager.Instance.runner.Despawn(gameObject.GetComponent<NetworkObject>());
    }
}
