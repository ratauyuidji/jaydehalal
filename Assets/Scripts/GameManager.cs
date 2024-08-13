using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject weapon;

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
            StartCoroutine(CheckAfterLastShoot());
        }
    }
    private IEnumerator CheckAfterLastShoot()
    {
        yield return new WaitForSeconds(3f);
        if (enemylist.Count == 0)
        {
            Debug.Log("Win");
            WinGame();
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
        StartCoroutine(CheckAllEnemyDeath());
    }
    private IEnumerator CheckAllEnemyDeath()
    {
        yield return new WaitForSeconds(3f);
        if(enemylist.Count == 0)
        {
            Debug.Log("Win");
            WinGame();
        }
    }
    public void WinGame()
    {
        win.SetActive(true);
        weapon.SetActive(false);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
