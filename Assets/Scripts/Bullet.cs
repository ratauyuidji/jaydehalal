using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bounceNumber = 3;
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
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("FallBox"))
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
}
