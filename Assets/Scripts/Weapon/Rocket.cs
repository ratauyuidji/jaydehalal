using UnityEngine;

public class Rocket : Projectile
{
    public float speed = 8f;
    public float force = 700f;
    public float radius = 5f;
    public GameObject deathVFXPrefab;

    public override void Shoot(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
        Destroy(this.gameObject);
    }

    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            if (obj.gameObject.CompareTag("Enemy"))
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
    }
}
