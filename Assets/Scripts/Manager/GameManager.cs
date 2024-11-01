using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hapiga.Ads;
using Hapiga.Tracking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IconHandler iconHandler;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private int maxReloadTime = 10;
    [SerializeField] private TextMeshProUGUI reloadText;
    [SerializeField] private TextMeshProUGUI moneyEarnedText;
    [SerializeField] private GameObject economyManagerPrefab;
    [SerializeField] private CanvasGroup backgroundUI;
    [SerializeField] private GameObject addBazookaPanel;
    [SerializeField] private GameObject addNadePanel;
    [SerializeField] private GameObject addBulletPanel;
    [SerializeField] private GameObject addSkipPanel;

    [SerializeField] private Animator transitionAnim;

    Coroutine CWin;
    Coroutine CCheckEnemy;
    private const string ReloadTimeKey = "CurrentReloadTime";
    public static GameManager Instance;
    public int maxNumberOfShoot;
    private int useNumberOfShoot;
    private List<Enemy> enemylist = new List<Enemy>(); 
    private int currentStarsNum = 0;
    private int levelIndex;
    public StarDisplay starDisplay;
    private int currentReloadTime;
    private bool canReload = true;
    public bool hasWon = false;
    private bool hasLost = false;
    private GameObject playerarm;
    public Button x2Button;

    public GameObject[] levelPrefabs;
    private GameObject currentLevel;
    public Button addAmmoButton;
    public Button addNadeButton;
    public Button switchBazookaButton;
    public Button switchNadeButton;


    private void Start()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        
    }
    public void SetUpWhenLoadLevel()
    {
        levelIndex = LevelManager.Instance.GetLevelIndex();
        Debug.Log("level index get from levelmanager" + levelIndex);
        if (PlayerPrefs.HasKey(ReloadTimeKey))
        {
            currentReloadTime = PlayerPrefs.GetInt(ReloadTimeKey);
        }
        else
        {
            currentReloadTime = 0;
        }
        UpdateReloadText();
        if (EconomyManager.Instance == null)
        {
            Instantiate(economyManagerPrefab);
        }
        hasLost = false;
        hasWon = false;
        useNumberOfShoot = 0;
        iconHandler.ResetIcons();
        win.SetActive(false);
        lose.SetActive(false);

        if (backgroundUI != null)
        {
            backgroundUI.interactable = true;
            backgroundUI.blocksRaycasts = true;
        }
        FindEnemyAndWeapon();
        starDisplay.ResetStars();
        //iconHandler.SetMaxNumberOfShoot(5);
        Debug.Log("maxNumberOfShoot" + maxNumberOfShoot);
    }
    private void Update()
    {
        if (InputManager.wasLeftMouseButtonPressed)
        {
            RaycastForCanFire();
        }

    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void FindEnemyAndWeapon()
    {
        enemylist.Clear();
        Enemy[] enemy = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemy.Length; i++)
        {
            enemylist.Add(enemy[i]);
            Debug.Log("Added enemy to list: " + enemy[i].name);
        }
        Debug.Log("Total number of enemies in list: " + enemylist.Count);

        var weaponScript = FindObjectOfType<Weapon>();
        if (weaponScript != null)
        {
            playerarm = weaponScript.gameObject;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng Player có gắn script Weapon");
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
            currentReloadTime = Mathf.Clamp(currentReloadTime - 1, 0, maxReloadTime);
            UpdateReloadText();
            SaveReloadTime();
            if (currentReloadTime == 0)
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
            LoseGame();
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemylist.Remove(enemy);
        StartCoroutine(CheckAllEnemyDeath());
    }
    private IEnumerator CheckAllEnemyDeath()
    {
        yield return new WaitForSeconds(1.5f);
        if (enemylist.Count == 0)
        {
            Debug.Log("Win");
            Debug.Log(useNumberOfShoot);
            WinGame();
        }
    }
    public void WinGame()
    {
        if (hasWon || hasLost) return;
        hasWon = true;

        int remainShoot = maxNumberOfShoot - useNumberOfShoot;
        remainShoot = Mathf.Clamp(remainShoot, 0, 3);
        int moneyEarned = remainShoot * 10;
        EconomyManager.Instance.IncreaseMoney(moneyEarned);
        Debug.Log("money: " + moneyEarned);
        StartCoroutine(AnimateMoneyText(moneyEarned));
        CheckStar(remainShoot);
        PlayerPrefs.SetInt("Level" + levelIndex + "_Win", 1);
        win.SetActive(true);
        TextMeshProUGUI winText = win.transform.Find("WinText").GetComponent<TextMeshProUGUI>();
        if (winText != null)
        {
            switch (remainShoot)
            {
                case 3:
                    winText.text = "FANTASTIC!";
                    break;
                case 2:
                    winText.text = "AWESOME!";
                    break;
                case 1:
                    winText.text = "WELL DONE!";
                    break;
                case 0:
                    winText.text = "GOOD!";
                    break;
            }
        }
        starDisplay.DisplayStar(remainShoot);
        playerarm.SetActive(false);

        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }

        CompleteLevel(levelIndex);
        if (levelIndex >= 3)
        {
            AdManager.Instance.ShowInterstitialAds(null, false);
        }
    }
    public void CompleteLevel(int level)
    {
        TrackingManager.TrackEvent($"completed_level_{level:000}");
        Debug.Log(level);
        if (level == 64)
        {
            //TrackingManager.TrackEvent($"archived_level_{level:000}");
        }
    }
    private IEnumerator AnimateMoneyText(int moneyEarned)
    {
        float duration = 1.0f;
        float elapsedTime = 0;
        int currentMoney = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentMoney = Mathf.FloorToInt(Mathf.Lerp(0, moneyEarned, elapsedTime / duration));
            moneyEarnedText.text = "+" + currentMoney.ToString();
            yield return null;
        }

        moneyEarnedText.text = "+" + moneyEarned.ToString();
    }
    public void MultiplyMoney()
    {
        AdManager.Instance.ShowRewardedVideo(CloseRewardCallback);
    }
    void CloseRewardCallback()
    {
        if (moneyEarnedText != null)
        {
            int currentMoney = int.Parse(moneyEarnedText.text.Replace("+", ""));
            int doubledMoney = currentMoney * 2;

            StartCoroutine(AnimateMultiplyMoney(currentMoney, doubledMoney));
            x2Button.gameObject.SetActive(false);
        }
    }


    private IEnumerator AnimateMultiplyMoney(int currentMoney, int targetMoney)
    {
        float duration = 1.0f; 
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            int newMoney = Mathf.FloorToInt(Mathf.Lerp(currentMoney, targetMoney, elapsedTime / duration));
            moneyEarnedText.text = "+" + newMoney.ToString();
            yield return null;
        }

        moneyEarnedText.text = "+" + targetMoney.ToString();
        EconomyManager.Instance.IncreaseMoney(targetMoney - currentMoney);
    }
    public void LoseGame()
    {
        if (hasWon || hasLost) return;
        hasLost = true;

        lose.SetActive(true);
        CheckStar(0);
        playerarm.SetActive(false);
        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }
    }
    public void SkipLevel()
    {
        AdManager.Instance.ShowRewardedVideo(CloseRewardCallbackSkip);
    }
    void CloseRewardCallbackSkip()
    {
        if (lose.activeSelf)
        {
            lose.SetActive(false);
        }
        EconomyManager.Instance.IncreaseMoney(0);
        Debug.Log("money: " + 0);
        if (moneyEarnedText != null)
        {
            moneyEarnedText.text = "+" + 0;
        }
        CheckStar(0);
        PlayerPrefs.SetInt("Level" + levelIndex + "_Win", 1);
        win.SetActive(true);
        starDisplay.DisplayStar(0);
        playerarm.SetActive(false);
        addSkipPanel.SetActive(false);
        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }
    }
    public void RestartGame()
    {
        ChangeWeapon changeWeapon = FindObjectOfType<ChangeWeapon>();
        changeWeapon.PrepareForNextLevel();
        transitionAnim.SetTrigger("End");
        LevelManager.Instance.LoadLevel(levelIndex-1);
        Debug.Log("level index khi ++" + levelIndex);
        transitionAnim.SetTrigger("Start");
    }
    public void NextLevel()
    {
        ChangeWeapon changeWeapon = FindObjectOfType<ChangeWeapon>();
        changeWeapon.PrepareForNextLevel();
        transitionAnim.SetTrigger("End");
        LevelManager.Instance.LoadLevel(levelIndex);
        Debug.Log("level index khi ++" + levelIndex);
        transitionAnim.SetTrigger("Start"); 
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
    public void AddReloadTime()
    {
        AdManager.Instance.ShowRewardedVideo(CloseRewardCallbackReload);
    }
    void CloseRewardCallbackReload()
    {
        currentReloadTime = Mathf.Clamp(currentReloadTime + 1, 0, maxReloadTime);
        if (currentReloadTime > 0)
        {
            canReload = true;
        }
        UpdateReloadText();
        SaveReloadTime();
        TurnOffAddPanel();
    }

    private void UpdateReloadText()
    {
        if (reloadText != null)
        {
            reloadText.text = currentReloadTime.ToString();
        }
    }
    private void SaveReloadTime()
    {
        PlayerPrefs.SetInt(ReloadTimeKey, currentReloadTime);
        PlayerPrefs.Save();
    }
    public bool RaycastForCanFire()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            //Debug.Log("Raycast hit: " + hit.collider.name);
            if (hit.collider.CompareTag("CanFire"))
            {
                return true;
            }
        }
        //Debug.Log("Raycast did not hit the specified object.");
        return false;
    }
    public void TurnOnAddPanel(string panelName)
    {
        DisableAllPanels();
        switch (panelName)
        {
            case "GetBazookaPanel":
                addBazookaPanel.SetActive(true);
                break;
            case "GetNadePanel":
                addNadePanel.SetActive(true);
                break;
            case "GetBulletPanel":
                addBulletPanel.SetActive(true);
                break;
            case "GetSkipPanel":
                addSkipPanel.SetActive(true);
                break;
        }
        playerarm.SetActive(false);
    }

    public void TurnOffAddPanel()
    {
        DisableAllPanels();
        StartCoroutine(TurnOnPlayerGun());
    }
    IEnumerator TurnOnPlayerGun()
    {
        yield return new WaitForSeconds(0.5f);
        playerarm.SetActive(true);
    }
    private void DisableAllPanels()
    {
        addBazookaPanel.SetActive(false);
        addNadePanel.SetActive(false);
        addBulletPanel.SetActive(false);
        addSkipPanel.SetActive(false);
    }
    
}
