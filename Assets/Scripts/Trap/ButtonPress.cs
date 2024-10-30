using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Bom bom;
    public GameObject trapObject;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bom != null)
        {
            bom.Explode();
            Destroy(bom.gameObject);
        }

        if (trapObject != null)
        {
            Destroy(trapObject.gameObject);
        }

        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Now the condition is triggered by any collider
        if (bom != null)
        {
            bom.Explode();
            Destroy(bom.gameObject);
        }

        if (trapObject != null)
        {
            Destroy(trapObject.gameObject);
        }
        Destroy(this.gameObject);
    }
}
