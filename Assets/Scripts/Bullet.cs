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
        //if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("FallBox") || other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Joint"))
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
        if (other.gameObject.CompareTag("Joint"))
        {
            Destroy(other.gameObject);
        }
    }
}
