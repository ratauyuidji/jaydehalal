using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    private int totalWeapon = 1;
    public int currentWeaponIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;

    private GameObject gun2Background;  
    private GameObject nadeBackground;

    private void Start()
    {
        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];

        for (int i = 0; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }
        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;

        gun2Background = GameObject.Find("Gun2Background");
        nadeBackground = GameObject.Find("NadeBackground");

        if (gun2Background != null)
            gun2Background.SetActive(false);
        if (nadeBackground != null)
            nadeBackground.SetActive(false);
    }


    private void Update()
    {

    }

    public void SwitchToGun2()
    {
        if (currentWeaponIndex != 1)
        {
            guns[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = 1;
            guns[currentWeaponIndex].SetActive(true);
            currentGun = guns[currentWeaponIndex];

            // Bật background của Gun 2
            if (gun2Background != null)
                gun2Background.SetActive(true);
            
            if (nadeBackground != null)
                nadeBackground.SetActive(false);
        }
    }

    public void SwitchToNade()
    {
        if (currentWeaponIndex != 2)
        {
            guns[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = 2;
            guns[currentWeaponIndex].SetActive(true);
            currentGun = guns[currentWeaponIndex];

            // Bật background của Nade
            if (nadeBackground != null)
                nadeBackground.SetActive(true);
            
            if (gun2Background != null)
                gun2Background.SetActive(false);
        }
    }

    public void SwitchToFirstWeapon()
    {
        guns[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = 0;
        guns[currentWeaponIndex].SetActive(true);
        currentGun = guns[currentWeaponIndex];

        
        if (gun2Background != null)
            gun2Background.SetActive(false);
        if (nadeBackground != null)
            nadeBackground.SetActive(false);
    }
}
