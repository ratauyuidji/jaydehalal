using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class MapSelection : MonoBehaviour
{
    public GameObject unlocked;
    public GameObject locked;

    public int questNum; //require star to unlock
    public int startLevel;
    public int endLevel;
    public bool isUnlock = false;

    
    private void Update()
    {
        UnlockMap();
        UpdateMapStatus();
    }
    public void UpdateMapStatus()
    {
        if (isUnlock) //unlock
        {
            unlocked.gameObject.SetActive(true);
            locked.gameObject.SetActive(false);
        }
        else //lock
        {
            //unlocked.gameObject.SetActive(false);
            locked.gameObject.SetActive(true);
        }
    }
    public void UnlockMap()
    {

        if (UIManager.Instance.stars >= questNum)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock=false;
        }
    }
}
