using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

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
            Vector2 pos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
            return pos;
        }
    }

    private void Start()
    {
        changeWeapon = FindObjectOfType<ChangeWeapon>();
    }

    private void Update()
    {
        if (TouchUI.IsPointerOverUI())
            return;

        if (InputManager.wasLeftMouseButtonReleased && GameManager.Instance.RaycastForCanFire())
        {
            if (GameManager.Instance.HasEnoughShoot())
            {
                Shoot();
                GameManager.Instance.UseShoot();
                laser.DeactivateLaser();
            }
        }
    }

    private void Shoot()
    {
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(InputManager.MousePosition) - (Vector2)transform.position;
        Projectile projectile = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
        projectile.Shoot(direction.normalized);
        laser.DeactivateLaser();
    }

    private IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
