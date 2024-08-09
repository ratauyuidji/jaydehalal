using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private Rigidbody2D rb;

    private bool isFalling = false;
    private void Update()
    {
        if(rb.velocity.y < -2 && !isFalling)
        {
            isFalling = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("FallBox"))
        {
            GameManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Wall") && isFalling)
        {
            GameManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        }
    }
}
