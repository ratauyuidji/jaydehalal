using UnityEngine;

public class Rocket : Projectile
{
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
            else
            {
                explosive.ApplyForceToObject(obj);
            }
        }
    }
}