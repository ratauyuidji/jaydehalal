using UnityEngine;

public class EnemyChildren : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    
    private Enemy parentEnemy;

    private void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("CanDestroyBox") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
        {
            float impactVelocity = other.relativeVelocity.magnitude;
            if (impactVelocity > parentEnemy.damageThreshold)
            {
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
            if (impactVelocity > parentEnemy.damageThreshold && !parentEnemy.isDied)
            {
                parentEnemy.TakeDamage(impactVelocity);
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected.");
            parentEnemy.OnBulletHit(other);
            Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
        } 
    }
    
}
