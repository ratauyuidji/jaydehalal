using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nade : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject destroyVFXPrefab;

    private Rigidbody2D rb;
    private Explosive explosive;
    public float radius = 5f;
    private bool hasExploded = false;
    private Coroutine explosionCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        explosive = GetComponent<Explosive>();
    }

    private void Start()
    {
        explosionCoroutine = StartCoroutine(ExplosionCountdown(3.5f));
    }

    private void OnDestroy()
    {
        if (explosionCoroutine != null)
        {
            StopCoroutine(explosionCoroutine);
        }
    }

    private IEnumerator ExplosionCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D obj in objects)
        {
            if (obj == null) continue;

            Bom bom = obj.GetComponent<Bom>();
            if (bom != null)
            {
                bom.Explode();
                Destroy(bom.gameObject);
            }

            Nade nade = obj.GetComponent<Nade>();
            if (nade != null && !nade.hasExploded)
            {
                nade.Explode();
                Destroy(nade.gameObject);
            }
            else if (obj.gameObject.CompareTag("Enemy") && explosive != null)
            {
                explosive.HandleEnemy(obj);
            }
            else if (obj.gameObject.CompareTag("Hostages") && explosive != null)
            {
                explosive.HandleHostage(obj);
            }
            else if (obj.gameObject.CompareTag("CanDestroyBox"))
            {
                Instantiate(destroyVFXPrefab, obj.transform.position, Quaternion.identity);
                Destroy(obj.gameObject);
            }
            else if (explosive != null)
            {
                explosive.ApplyForceToObject(obj);
            }
        }

        Destroy(gameObject);
    }
}
