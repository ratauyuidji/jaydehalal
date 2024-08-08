using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("FallBox"))
        {
            Destroy(gameObject);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        }
    }
}
