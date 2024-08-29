using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject player;
    [SerializeField] private int maxReloadTime = 10;
    [SerializeField] private TextMeshProUGUI reloadText;
    [SerializeField] private TextMeshProUGUI moneyEarnedText;
    [SerializeField] private GameObject economyManagerPrefab;


    Coroutine CWin;
    Coroutine CCheckEnemy;

    public static GameManager Instance;
    public int maxNumberOfShoot;
    private int useNumberOfShoot;
    private List<Enemy> enemylist = new List<Enemy>(); 
    private int currentStarsNum = 0;
    public int levelIndex;
    public StarDisplay starDisplay;
    private int currentReloadTime;
    private bool canReload = true;
    private bool hasWon = false;



    private void Start()
    {
        currentReloadTime = 1;
        UpdateReloadText();
        if (EconomyManager.Instance == null)
        {
            Instantiate(economyManagerPrefab); // economyManagerPrefab là prefab chứa EconomyManager
        }
    }
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
    public void PlusBullet()
    {
        if (useNumberOfShoot > 0 && canReload)
        {
            useNumberOfShoot--;
            currentReloadTime--;
            UpdateReloadText();
            if(currentReloadTime == 0)
            {
                canReload = false;
            }
            iconHandler.PlusShot(useNumberOfShoot);
            Debug.Log("Số lần bắn còn lại: " + (maxNumberOfShoot - useNumberOfShoot));
        }
        else
        {
            Debug.Log("Không thể thêm");
        }
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
        if (hasWon) return;
        hasWon = true;

        int remainShoot = maxNumberOfShoot - useNumberOfShoot;
        remainShoot = Mathf.Clamp(remainShoot, 0, 3);
        int moneyEarned = remainShoot * 10;
        EconomyManager.Instance.IncreaseMoney(moneyEarned);
        Debug.Log("money: " + moneyEarned);
        if (moneyEarnedText != null)
        {
            moneyEarnedText.text = "+" + moneyEarned.ToString();
        }
        CheckStar(remainShoot);
        win.SetActive(true);
        starDisplay.DisplayStar(remainShoot);
        player.SetActive(false);
    }
    public void SkipLevel()
    {
        EconomyManager.Instance.IncreaseMoney(30);
        Debug.Log("money: " + 30);
        if (moneyEarnedText != null)
        {
            moneyEarnedText.text = "+" + 30;
        }
        CheckStar(3);
        win.SetActive(true);
        starDisplay.DisplayStar(3);
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
    public void AddReloadTime(int reload)
    {
        currentReloadTime = Mathf.Clamp(currentReloadTime + reload, 0, maxReloadTime);
        if (currentReloadTime > 0)
        {
            canReload = true;
        }
        UpdateReloadText();
    }

    private void UpdateReloadText()
    {
        if (reloadText != null)
        {
            reloadText.text = currentReloadTime.ToString();
        }
    }


}
