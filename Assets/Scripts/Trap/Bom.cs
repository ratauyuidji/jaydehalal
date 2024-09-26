using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] public float damageThreshold = 1f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject destroyVFXPrefab;

    private float currentHealth;
    public float radius;
    private bool hasExploded = false;
    private Explosive explosive; 

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        explosive = GetComponent<Explosive>();
    }
    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion,1f);

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            Bom otherBomb = obj.GetComponent<Bom>();
            if (otherBomb != null && otherBomb != this)
            {
                otherBomb.Explode();
                Destroy(otherBomb.gameObject);
            }
            else if (obj.gameObject.CompareTag("Enemy"))
            {
                explosive.HandleEnemy(obj);
            }
            else if (obj.gameObject.CompareTag("Hostages"))
            {
                explosive.HandleHostage(obj);
            }
            else if (obj.gameObject.CompareTag("CanDestroyBox"))
            {
                Instantiate(destroyVFXPrefab, obj.transform.position, Quaternion.identity);
                Destroy(obj.gameObject);
            }
            else if(explosive != null)
{
                explosive.ApplyForceToObject(obj);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Explode();
            Destroy(this.gameObject);
        }
        else
        {
            float impactVelocity = other.relativeVelocity.magnitude;
            if (impactVelocity > damageThreshold)
            {
                Debug.Log(impactVelocity);
                TakeDamage(impactVelocity);
            }
        }
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("takedame");
        if (currentHealth <= 0)
        {
            Explode();
            Destroy(this.gameObject);
        }
    }
}
