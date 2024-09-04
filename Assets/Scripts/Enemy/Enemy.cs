using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] public float damageThreshold = 1f;
    private float currentHealth;
    private bool isDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDied = false;
    }
    


    public void TakeDamage(float damageAmount)
    {
        if (isDied) return;
        currentHealth -= damageAmount;
        Debug.Log("takedame");
        if (currentHealth <= 0)
        {
            isDied = true;
            GameManager.Instance.RemoveEnemy(this);
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if (impactVelocity > damageThreshold)
        {
            TakeDamage(impactVelocity);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.Instance.RemoveEnemy(this);
            Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
        }
    }*/
    public void OnBulletHit(Collider2D other)
    {
        GameManager.Instance.RemoveEnemy(this);
    }
}
