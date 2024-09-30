using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
    private GameObject player;

    private void Start()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        string sceneName = SceneManager.GetActiveScene().name;

        levelIndex = ExtractLevelIndex(sceneName);
        Debug.Log("Level Index: " + levelIndex);
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
        Weapon weaponScript = FindObjectOfType<Weapon>();

        if (weaponScript != null)
        {
            player = weaponScript.gameObject;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng Player có gắn script Weapon");
        }
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
        Enemy[] enemy = FindObjectsOfType<Enemy>();
        for(int i = 0; i < enemy.Length; i++)
        {
            enemylist.Add(enemy[i]);
        }
    }
    private int ExtractLevelIndex(string sceneName)
    {
        for (int i = 0; i < sceneName.Length; i++)
        {
            if (char.IsDigit(sceneName[i]))
            {
                return int.Parse(sceneName.Substring(i));
            }
        }
        return 0;
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
        if (moneyEarnedText != null)
        {
            moneyEarnedText.text = "+" + moneyEarned.ToString();
        }
        
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
        player.SetActive(false);

        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }
    }
    public void LoseGame()
    {
        if (hasWon || hasLost) return;
        hasLost = true;

        lose.SetActive(true);
        CheckStar(0);
        player.SetActive(false);
        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }
    }
    public void SkipLevel()
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
        player.SetActive(false);
        if (backgroundUI != null)
        {
            backgroundUI.interactable = false;
            backgroundUI.blocksRaycasts = false;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        transitionAnim.SetTrigger("End");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("Start");
    }
    IEnumerator LoadLevel()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(0.00001f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    public void AddReloadTime(int reload)
    {
        currentReloadTime = Mathf.Clamp(currentReloadTime + reload, 0, maxReloadTime);
        if (currentReloadTime > 0)
        {
            canReload = true;
        }
        UpdateReloadText();
        SaveReloadTime();
        GameManager.Instance.TurnOffAddPanel();
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
        Debug.Log("Raycast did not hit the specified object.");
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
        }
        player.SetActive(false);
    }

    public void TurnOffAddPanel()
    {
        DisableAllPanels();
        player.SetActive(true);
    }
    private void DisableAllPanels()
    {
        addBazookaPanel.SetActive(false);
        addNadePanel.SetActive(false);
        addBulletPanel.SetActive(false);
    }


}
