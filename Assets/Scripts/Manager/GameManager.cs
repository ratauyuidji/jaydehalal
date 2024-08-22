using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject player;
    Coroutine CWin;
    Coroutine CCheckEnemy;

    public static GameManager Instance;
    public int maxNumberOfShoot;
    private int useNumberOfShoot;
    private List<Enemy> enemylist = new List<Enemy>(); 
    private int currentStarsNum = 0;
    public int levelIndex;
    public StarDisplay starDisplay;

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
            Debug.Log(useNumberOfShoot);
            WinGame();
        }
        else
        {
            Debug.Log("Loss");
            lose.SetActive(true);
            CheckStar(0);
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
        if (enemylist.Count == 0)
        {
            Debug.Log("Win");
            Debug.Log(useNumberOfShoot);
            WinGame();
        }
    }
    public void WinGame()
    {
        int remainShoot = maxNumberOfShoot - useNumberOfShoot;
        CheckStar(remainShoot);
        win.SetActive(true);
        starDisplay.DisplayStar(remainShoot);
        player.SetActive(false);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void CheckStar(int starsNum)
    {
        currentStarsNum = starsNum;
        if(currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, currentStarsNum);
        }
        Debug.Log("star is " + PlayerPrefs.GetInt("Lv" + levelIndex, starsNum));
    }
    
}
