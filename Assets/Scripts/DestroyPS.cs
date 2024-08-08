using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPS : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    private void Update()
    {
        if (ps && !ps.IsAlive())
        {
            DestroySelf();
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
