using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    public float radius;
    public float force;
    private bool hasExploded = false;

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

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
                Enemy enemy = obj.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    GameManager.Instance.RemoveEnemy(enemy);
                    Instantiate(deathVFXPrefab, obj.transform.position, Quaternion.identity);

                    Rigidbody2D rb = enemy.GetComponentInChildren<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = rb.transform.position - transform.position;
                        rb.AddForce(direction.normalized * force);
                        Debug.Log(rb.transform.name);
                    }
                }
            }
            else
            {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = rb.transform.position - transform.position;
                    rb.AddForce(direction.normalized * force);
                }
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
    }
}
