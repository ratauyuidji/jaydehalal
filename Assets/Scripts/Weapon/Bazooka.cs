using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bazooka : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float shootDelay;
    [SerializeField] private Laser laser;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private TextMeshProUGUI ammoText;
    private ChangeWeapon changeWeapon;

    private int currentAmmo;
    private bool canShoot = true;

    private void Awake()
    {
        currentAmmo = 1;
        UpdateAmmoText();
        changeWeapon = FindObjectOfType<ChangeWeapon>();
        Debug.Log(currentAmmo);
    }
    
    private void OnEnable()
    {
        if (currentAmmo > 0)
        {
            canShoot = true;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;
                UpdateAmmoText();
                if (currentAmmo == 0)
                {
                    canShoot = false;
                }
            }
        }
    }

    private void Shoot()
    {
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        Projectile projectile = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
        projectile.Shoot(direction.normalized);
        canShoot = false;
        StartCoroutine(WaitForNextShot());
        laser.DeactivateLaser();
        if (changeWeapon.currentWeaponIndex == 1)
        {
            changeWeapon.SwitchToFirstWeapon();
        }
    }

    private IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public void AddAmmo(int ammo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + ammo, 0, maxAmmo);
        if (currentAmmo > 0)
        {
            canShoot = true;
        }
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}
