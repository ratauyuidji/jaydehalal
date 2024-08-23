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

    private void Start()
    {
        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];

        for(int i = 0; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }
        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentWeaponIndex < totalWeapon - 1)
            {
                guns[currentWeaponIndex].SetActive(false);
                currentWeaponIndex++;
                guns[currentWeaponIndex].SetActive(true);
                currentGun = guns[currentWeaponIndex];
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentWeaponIndex > 0)
            {
                guns[currentWeaponIndex].SetActive(false);
                currentWeaponIndex--;
                guns[currentWeaponIndex].SetActive(true);
                currentGun = guns[currentWeaponIndex];
            }
        }
    }
}
