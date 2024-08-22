using System.Collections;
using System.Collections.Generic;
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
        UpdateMapStatus();
        UnlockMap();
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
    private void UnlockMap()
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
