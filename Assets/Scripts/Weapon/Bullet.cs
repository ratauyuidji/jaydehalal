using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bounceNumber;
    [SerializeField] private float speed;


    private Vector2 direction;

    public void Shoot(Vector2 direction)
    {
        this.direction = direction;
        rb.velocity = direction * speed;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string[] validTags = { "Wall", "FallBox", "Box", "Joint" };
        if (System.Array.Exists(validTags, tag => tag == other.gameObject.tag))
        {
            bounceNumber--;
            if (bounceNumber < 0)
            {
                Destroy(gameObject);
                return;
            }
            var firstContact = other.contacts[0];
            Vector2 newVelocity = Vector2.Reflect(direction.normalized, firstContact.normal);
            Shoot(newVelocity.normalized);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("aaaa");
            Rigidbody2D erb  = other.GetComponent<Rigidbody2D>();
            Vector2 impactDirection = transform.position - other.transform.position;
            erb.AddForce(impactDirection * 40f,ForceMode2D.Impulse);
        }
        if (other.gameObject.CompareTag("Joint"))
        {
            Destroy(other.gameObject);
        }


        /*if (other.gameObject.CompareTag("WallTrigger"))
        {
            circleCollider.isTrigger = false;
        }
        if (other.gameObject.CompareTag("EnemyTrigger"))
        {
            circleCollider.isTrigger = true;
        }*/
    }
}