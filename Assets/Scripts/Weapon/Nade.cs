using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nade : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 800f;
    public float radius = 5f;
    public GameObject deathVFXPrefab;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            Bom bom = obj.GetComponent<Bom>();
            if (bom != null)
            {
                bom.Explode();
                Destroy(bom.gameObject);
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
        Destroy(gameObject);
    }
}
