using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;

    public static GameManager Instance;
    public int maxNumberOfShoot = 3;
    private int useNumberOfShoot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void UseShoot()
    {
        useNumberOfShoot++;
        iconHandler.UseShot(useNumberOfShoot);
    }
    public bool HasEnoughShoot()
    {
        if(useNumberOfShoot < maxNumberOfShoot)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
