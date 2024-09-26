using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostagesChild : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;

    private Hostages parentHostages;

    private void Start()
    {
        parentHostages = GetComponentInParent<Hostages>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("CanDestroyBox") || other.gameObject.CompareTag("Wall"))
        {
            float impactVelocity = other.relativeVelocity.magnitude;
            if (impactVelocity > parentHostages.damageThreshold)
            {
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
            if (impactVelocity > parentHostages.damageThreshold && !parentHostages.isDied)
            {
                Debug.Log(impactVelocity);
                parentHostages.TakeDamage(impactVelocity);
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            parentHostages.ActivateDeadSprite();
            Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
