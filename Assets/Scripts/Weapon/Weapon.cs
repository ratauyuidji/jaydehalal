using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float shootDelay;
    [SerializeField] private Laser laser;

    private bool canShoot = true;
    private ChangeWeapon changeWeapon;

    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return pos;
        }
    }

    private void Start()
    {
        changeWeapon = FindObjectOfType<ChangeWeapon>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.Instance.HasEnoughShoot())
            {
                Vector2 direction = MousePos - (Vector2)transform.position;
                Projectile projectile = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
                projectile.Shoot(direction.normalized);
                GameManager.Instance.UseShoot();
                /*canShoot = false;
                if (GameManager.Instance.HasEnoughShoot())
                {
                    StartCoroutine(WaitForNextShot());
                }*/
                laser.DeactivateLaser();

                if (changeWeapon.currentWeaponIndex == 1)
                {
                    changeWeapon.SwitchToFirstWeapon();
                }
            }

        }
    }
    private IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
