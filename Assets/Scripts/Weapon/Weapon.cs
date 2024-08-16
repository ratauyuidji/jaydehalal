using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float shootDelay;
    [SerializeField] private Laser laser;
    //SoundManager soundManager;
    private bool canShoot = true;

    private void Awake()
    {
        //soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
    }

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
        if(Input.GetMouseButtonUp(0) && canShoot)
        {
            if (GameManager.Instance.HasEnoughShoot())
            {
                Vector2 direction = MousePos - (Vector2)transform.position;
                Bullet bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
                //soundManager.PlaySFX(soundManager.bounce);
                //SoundManager.Instance.PlaySFX(SoundManager.Instance.gunShot);
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
