using UnityEngine;

public class EnemyChildren : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    private bool isDeathVFXEnabled = true;
    private Enemy parentEnemy;

    private void Start()
    {
        bool isVFXEnabled = PlayerPrefs.GetInt("ButtonState_3", 1) == 1;
        ToggleDeathVFX(isVFXEnabled);
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (parentEnemy == null)
        {
            parentEnemy = GetComponentInParent<Enemy>();
            if (parentEnemy == null)
            {
                Debug.LogWarning("Không tìm thấy đối tượng cha có component 'Enemy'.");
                return;
            }
        }

        if (other.gameObject.CompareTag("CanDestroyBox") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
        {
            float impactVelocity = other.relativeVelocity.magnitude;
            if (impactVelocity > parentEnemy.damageThreshold)
            {
                if (isDeathVFXEnabled)
                {
                    Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
                }
            }
            if (impactVelocity > parentEnemy.damageThreshold && !parentEnemy.isDied)
            {
                parentEnemy.TakeDamage(impactVelocity);
                if (isDeathVFXEnabled)
                {
                    Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected.");
            parentEnemy.OnBulletHit(other);
            if (isDeathVFXEnabled)
            {
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
        } 
    }
    public void ToggleDeathVFX(bool isEnabled)
    {
        isDeathVFXEnabled = isEnabled;
    }
}
