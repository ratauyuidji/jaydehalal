using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Bom bom;
    public GameObject trapObject1;
    public GameObject trapObject2;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bom != null)
        {
            bom.Explode();
            Destroy(bom.gameObject);
        }

        if (trapObject1 != null)
        {
            Destroy(trapObject1.gameObject);
        }
        if (trapObject2 != null)
        {
            trapObject2.SetActive(true);
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

        if (trapObject1 != null)
        {
            Destroy(trapObject1.gameObject);
        }
        if (trapObject2 != null)
        {
            trapObject2.SetActive(true);
        }
        Destroy(this.gameObject);
    }
}
