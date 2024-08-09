using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    //public GameObject loss;

    public static GameManager Instance;
    public int maxNumberOfShoot = 4;
    private int useNumberOfShoot;
    private List<Enemy> enemylist = new List<Enemy>(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Enemy[] enemy = FindObjectsOfType<Enemy>();
        for(int i = 0; i < enemy.Length; i++)
        {
            enemylist.Add(enemy[i]);
        }
    }
    
    public void UseShoot()
    {
        useNumberOfShoot++;
        iconHandler.UseShot(useNumberOfShoot);
        CheckLastShoot();
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
    public void CheckLastShoot()
    {
        if(useNumberOfShoot == maxNumberOfShoot)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }
    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(3f);
        if (enemylist.Count == 0)
        {
            Debug.Log("Win");
            win.SetActive(true);
        }
        else
        {
            Debug.Log("Loss");
            lose.SetActive(true);
        }

    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemylist.Remove(enemy);
        CheckAllEnemyDeath();
    }
    private void CheckAllEnemyDeath()
    {
        if(enemylist.Count == 0)
        {
            Debug.Log("Win");
            win.SetActive(true);
        }
    }
}
