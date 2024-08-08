using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float shootDelay;
    [SerializeField] private Laser laser;
    private bool canShoot = true;

    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return pos;
        }
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canShoot)
        {
            if (GameManager.Instance.HasEnoughShoot())
            {
                Vector2 direction = MousePos - (Vector2)transform.position;
                Bullet bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
                bullet.Shoot(direction.normalized);
                GameManager.Instance.UseShoot();
                canShoot = false;
                if (GameManager.Instance.HasEnoughShoot())
                {
                    StartCoroutine(WaitForNextShot());
                }
                laser.DeactivateLaser();
            }
            
        }
    }
    private IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
