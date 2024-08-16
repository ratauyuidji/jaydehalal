using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    //[SerializeField] private Enemy enemy;
    public float radius;
    public float force;
    public LayerMask layerToHit;

    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            if (obj.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    GameManager.Instance.RemoveEnemy(enemy);
                    Destroy(obj.gameObject);
                    Instantiate(deathVFXPrefab, obj.transform.position, Quaternion.identity);
                }
            }
            else
            {
                Vector2 direction = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
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
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            Destroy(this.gameObject);
        }
    }
}