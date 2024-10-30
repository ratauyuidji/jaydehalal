using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Hapiga.Ads;


public class Bazooka : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float shootDelay;
    [SerializeField] private Laser laser;
    [SerializeField] private int maxAmmo = 10;
    
    private TextMeshProUGUI ammoText;
    private ChangeWeapon changeWeapon;
    private int currentAmmo;
    private bool canShoot = true;
    private const string AmmoKey = "CurrentAmmo";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(AmmoKey))
        {
            currentAmmo = PlayerPrefs.GetInt(AmmoKey);
        }
        else
        {
            currentAmmo = 0;
        }
        
        UpdateAmmoText();
        changeWeapon = FindObjectOfType<ChangeWeapon>();
        Debug.Log(currentAmmo);
    }
    public void SetAmmoText(TextMeshProUGUI text)
    {
        ammoText = text;
        UpdateAmmoText();
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
        if (TouchUI.IsPointerOverUI())
            return;

        if (InputManager.wasLeftMouseButtonReleased && canShoot && GameManager.Instance.RaycastForCanFire())
        {
            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;
                UpdateAmmoText();
                SaveAmmo();
                if (currentAmmo == 0)
                {
                    canShoot = false;
                }
            }
        }
    }

    private void Shoot()
    {
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(InputManager.MousePosition) - (Vector2)transform.position;
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

    public void AddAmmo()
    {
        AdManager.Instance.ShowRewardedVideo(CloseRewardCallback);
        
    }
    void CloseRewardCallback()
    {
        currentAmmo = Mathf.Clamp(currentAmmo + 1, 0, maxAmmo);
        if (currentAmmo > 0)
        {
            canShoot = true;
        }
        UpdateAmmoText();
        SaveAmmo();
        GameManager.Instance.TurnOffAddPanel();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }

    private void SaveAmmo()
    {
        PlayerPrefs.SetInt(AmmoKey, currentAmmo);
        PlayerPrefs.Save();
    }
    
}
