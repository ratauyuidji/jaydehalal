using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float force = 800f;
    public GameObject deathVFXPrefab;

    public void HandleEnemy(Collider2D obj)
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

    public void ApplyForceToObject(Collider2D obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = rb.transform.position - transform.position;
            rb.AddForce(direction.normalized * force);
        }
    }
}
