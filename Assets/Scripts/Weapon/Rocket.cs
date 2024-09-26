using UnityEngine;

public class Rocket : Projectile
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject destroyVFXPrefab;


    public float speed = 8f;
    public float radius = 5f;
    private Explosive explosive;

    private void Start()
    {
        explosive = GetComponent<Explosive>();
    }
    public override void Shoot(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
        Destroy(this.gameObject);
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            Bom bom = obj.GetComponent<Bom>();
            if (bom != null)
            {
                bom.Explode();
                Destroy(bom.gameObject);
            }
            else if (obj.gameObject.CompareTag("Enemy"))
            {
                explosive.HandleEnemy(obj);
            }
            else if (obj.gameObject.CompareTag("Hostages"))
            {
                explosive.HandleHostage(obj);
            }
            else if (obj.gameObject.CompareTag("CanDestroyBox"))
            {
                Instantiate(destroyVFXPrefab, obj.transform.position, Quaternion.identity);
                Destroy(obj.gameObject);
            }
            else
            {
                explosive.ApplyForceToObject(obj);
            }
        }
    }
}