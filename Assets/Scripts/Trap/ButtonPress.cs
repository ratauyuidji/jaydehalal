using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Bom bom;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            bom.Explode();
            Destroy(this.gameObject);
            Destroy(bom.gameObject);
        }
    }
}
