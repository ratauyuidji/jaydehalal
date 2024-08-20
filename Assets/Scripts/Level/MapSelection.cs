using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelection : MonoBehaviour
{
    public bool isUnlock = false;
    public GameObject unlocked;
    public GameObject locked;

    private void Update()
    {
        UpdateMapStatus();
    }
    private void UpdateMapStatus()
    {
        if(isUnlock) //unlock
        {
            unlocked.gameObject.SetActive(true);
            locked.gameObject.SetActive(false);
        }
        else //lock
        {
            unlocked.gameObject.SetActive(false);
            locked.gameObject.SetActive(true);
        }
    }
}
