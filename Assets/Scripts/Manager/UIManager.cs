using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hapiga.Ads;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI completedLevelsMap1Text;
    [SerializeField] private TextMeshProUGUI completedLevelsMap2Text;
    [SerializeField] private TextMeshProUGUI completedLevelsMap3Text;
    [SerializeField] private TextMeshProUGUI completedLevelsMap4Text;

    [SerializeField] private TextMeshProUGUI starMap1Text;
    [SerializeField] private TextMeshProUGUI starMap2Text;
    [SerializeField] private TextMeshProUGUI starMap3Text;
    [SerializeField] private TextMeshProUGUI starMap4Text;



    public static UIManager Instance;
    public GameObject mapSelectionPanel;
    public GameObject otherMissionPanel;
    public GameObject[] levelSelectionPanels;
    public GameObject settingPanel;
    public GameObject shopPanel;
    public int stars;
    public MapSelection[] mapSelection;
    public TextMeshProUGUI[] questStarsText;
    public TextMeshProUGUI[] lockedStarsText;
    public TextMeshProUGUI[] unlockedStarsText;
    public TextMeshProUGUI startText;
    public Image[] checkmarks;
    private Vector3[] initialPositions;
    private int starMap1;
    private int starMap2;
    private int starMap3;
    private int starMap4;



    private void Awake()
    {
        TransferAndDeletePlayerPrefs();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        initialPositions = new Vector3[levelSelectionPanels.Length];
        for (int i = 0; i < levelSelectionPanels.Length; i++)
        {
            initialPositions[i] = levelSelectionPanels[i].GetComponent<RectTransform>().anchoredPosition;
        }
    }
    private bool[] buttonStates;

    void Start()
    {
        //AdManager.Instance.ShowBanner();
        EconomyManager.Instance.AssignMoneyText();
    }
    
    private void Update()
    {
        UpdateLockedStarUI();
        UpdateUnlockedStarUI();
        UpdateStarUI();
        UpdateCompletedLevelsText();
    }
    private void UpdateCompletedLevelsText()
    {
        int completedLevels1Count = GetCompletedLevels1Count();
        int completedLevels2Count = GetCompletedLevels2Count();
        int completedLevels3Count = GetCompletedLevels3Count();
        int completedLevels4Count = GetCompletedLevels4Count();

        completedLevelsMap1Text.text = completedLevels1Count.ToString() + "/" + "160";
        completedLevelsMap2Text.text = completedLevels2Count.ToString() + "/" + "32";
        completedLevelsMap3Text.text = completedLevels3Count.ToString() + "/" + "32";
        completedLevelsMap4Text.text = completedLevels4Count.ToString() + "/" + "32";
        starMap1Text.text = starMap1.ToString();
        starMap2Text.text = starMap2.ToString();
        starMap3Text.text = starMap3.ToString();
        starMap4Text.text = starMap4.ToString();
    }
    public void UpdateLockedStarUI()
    {
        for(int i = 0;i < mapSelection.Length;i++)
        {
            questStarsText[i].text = stars.ToString() + "/" + mapSelection[i].questNum.ToString();
            if (mapSelection[i].isUnlock == false)
            {
                lockedStarsText[i].text = stars.ToString() + "/" + mapSelection[i].endLevel * 3;
            }
        }
    }
    public void UpdateUnlockedStarUI()
    {
        for(int i = 0; i < mapSelection.Length; i++)
        {
            unlockedStarsText[i].text = stars.ToString();// + "/" + mapSelection[i].endLevel * 3;
            switch (i)
            {
                case 0:
                    int totalStars0 = 0;
                    for (int lv = 1; lv <= 160; lv++)
                    {
                        totalStars0 += PlayerPrefs.GetInt("Lv" + lv);
                    }
                    int maxStars = (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    unlockedStarsText[i].text = totalStars0.ToString();// + "/" + maxStars;
                    starMap1 = totalStars0;
                    break;

                case 1:
                    int totalStars1 = 0;
                    for (int lv = 1; lv <= 32; lv++)
                    {
                        totalStars1 += PlayerPrefs.GetInt("HLv" + lv);
                    }
                    unlockedStarsText[i].text = totalStars1.ToString();// + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    starMap2 = totalStars1;
                    break;
                case 2:
                    int totalStars2 = 0;
                    for (int lv = 1; lv <= 32; lv++)
                    {
                        totalStars2 += PlayerPrefs.GetInt("NLv" + lv);
                    }
                    unlockedStarsText[i].text = totalStars2.ToString();// + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    starMap3 = totalStars2;
                    break;
                case 3:
                    int totalStars3 = 0;
                    for (int lv = 1; lv <= 32; lv++)
                    {
                        totalStars3 += PlayerPrefs.GetInt("FLv" + lv);
                    }
                    unlockedStarsText[i].text = totalStars3.ToString();// + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    starMap4 = totalStars3;
                    break;
            }
        }
    }
    public void UpdateStarUI()
    {
        stars = 0;
        for (int i = 1; i <= 160; i++)
        {
            stars += PlayerPrefs.GetInt("Lv" + i);
        }
        for (int i = 1; i <= 32; i++)
        {
            stars += PlayerPrefs.GetInt("HLv" + i);
        }
        for (int i =1;  i <= 32; i++)
        {
            stars += PlayerPrefs.GetInt("NLv" + i);
        }
        startText.text = stars.ToString() + "/" + 768;
    }
    public void PressMapButton(int mapIndex)
    {
        if (mapSelection[mapIndex].isUnlock == true)
        {
            mapSelectionPanel.gameObject.SetActive(false);
            otherMissionPanel.gameObject.SetActive(false);
            StartCoroutine(ShowPanel(levelSelectionPanels[mapIndex], mapIndex));

            switch (mapIndex)
            {
                case 0: // Classic Levels
                    LevelSelection[] levels = levelSelectionPanels[mapIndex].GetComponentsInChildren<LevelSelection>();
                    foreach (LevelSelection level in levels)
                    {
                        level.UpdateLevelStatus();
                        level.UpdateLevelImage();
                    }
                    break;

                case 1: // Hostage Levels
                    HostageLevelSelection[] hlevels = levelSelectionPanels[mapIndex].GetComponentsInChildren<HostageLevelSelection>();
                    foreach (HostageLevelSelection hlevel in hlevels)
                    {
                        hlevel.UpdateLevelStatus();
                        hlevel.UpdateLevelImage();
                    }
                    break;

                case 2: // Nade Levels
                    NadeLevelSelection[] nlevels = levelSelectionPanels[mapIndex].GetComponentsInChildren<NadeLevelSelection>();
                    foreach (NadeLevelSelection nlevel in nlevels)
                    {
                        nlevel.UpdateLevelStatus();
                        nlevel.UpdateLevelImage();
                    }
                    break;

                case 3: // Friendly Fire Levels
                    FFLevelSelecton[] flevels = levelSelectionPanels[mapIndex].GetComponentsInChildren<FFLevelSelecton>();
                    foreach (FFLevelSelecton flevel in flevels)
                    {
                        flevel.UpdateLevelStatus();
                        flevel.UpdateLevelImage();
                    }
                    break;

                default:
                    Debug.LogWarning("Invalid mapIndex: " + mapIndex);
                    break;
            }
        }
        else
        {
            Debug.Log("You cannot open this map now");
        }
    }


    private IEnumerator ShowPanel(GameObject panel, int mapIndex)
    {
        panel.SetActive(true);

        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        Vector3 startPosition = new Vector3(0, -Screen.height, 0);
        Vector3 targetPosition = initialPositions[mapIndex];

        float elapsedTime = 0f;
        float duration = 0.5f;

        rectTransform.anchoredPosition = startPosition;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    public void BackMapButton()
    {
        foreach (MapSelection map in mapSelection)
        {
            map.UpdateMapStatus();
            map.UnlockMap();
        }
        mapSelectionPanel.gameObject.SetActive(true);
        otherMissionPanel.gameObject.SetActive(true);
        for(int i = 0;i < mapSelection.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }
    public void BackOtherMissionPanel()
    {

    }
    public void LoadHighestUnlockedLevelMode1()
    {
        LoadHighestUnlockedLevel(1, 160);
    }

    public void LoadHighestUnlockedLevelMode2()
    {
        LoadHighestHostageUnlockedLevel(1, 32);
    }
    public void LoadHighestUnlockedLevelMode3()
    {
        LoadHighestNadeUnlockedLevel(1, 32);
    }
    public void LoadHighestUnlockedLevelMode4()
    {
        LoadHighestFFUnlockedLevel(1, 32);
    }

    private void LoadHighestUnlockedLevel(int start, int end)
    {
        int highestUnlockedLevel = start;

        for (int i = start; i <= end; i++)
        {
            if (PlayerPrefs.GetInt("Lv" + i) >= 0 && PlayerPrefs.GetInt("Level" + i + "_Win") == 1)
            {
                highestUnlockedLevel = i+1;
            }
            else
            {
                break; // stop if level lock
            }
        }

        //string levelName = "Level" + highestUnlockedLevel;
        Debug.Log("Loading highest unlocked level: " + highestUnlockedLevel);
        //SceneManager.LoadScene(levelName);
        PlayerPrefs.SetInt("SelectedLevel", highestUnlockedLevel);
        PlayerPrefs.SetString("SelectedMode", "Classic");
        SceneManager.LoadScene(1);
    }
    private void LoadHighestHostageUnlockedLevel(int start, int end)
    {
        int highestUnlockedLevel = start;

        for (int i = start; i <= end; i++)
        {
            if (PlayerPrefs.GetInt("HLv" + i) >= 0 && PlayerPrefs.GetInt("HLevel" + i + "_Win") == 1)
            {
                highestUnlockedLevel = i + 1;
            }
            else
            {
                break; // stop if level lock
            }
        }
        Debug.Log("Loading highest unlocked level: " + highestUnlockedLevel);
        PlayerPrefs.SetInt("SelectedHostageLevel", highestUnlockedLevel);
        PlayerPrefs.SetString("SelectedMode", "Hostage");
        SceneManager.LoadScene(1);
    }
    private void LoadHighestNadeUnlockedLevel(int start, int end)
    {
        int highestUnlockedLevel = start;

        for (int i = start; i <= end; i++)
        {
            if (PlayerPrefs.GetInt("NLv" + i) >= 0 && PlayerPrefs.GetInt("NLevel" + i + "_Win") == 1)
            {
                highestUnlockedLevel = i + 1;
            }
            else
            {
                break; // stop if level lock
            }
        }
        Debug.Log("Loading highest unlocked level: " + highestUnlockedLevel);
        PlayerPrefs.SetInt("SelectedNadeLevel", highestUnlockedLevel);
        PlayerPrefs.SetString("SelectedMode", "Nade");
        SceneManager.LoadScene(1);
    }
    private void LoadHighestFFUnlockedLevel(int start, int end)
    {
        int highestUnlockedLevel = start;

        for (int i = start; i <= end; i++)
        {
            if (PlayerPrefs.GetInt("FLv" + i) >= 0 && PlayerPrefs.GetInt("FLevel" + i + "_Win") == 1)
            {
                highestUnlockedLevel = i + 1;
            }
            else
            {
                break; // stop if level lock
            }
        }
        Debug.Log("Loading highest unlocked level: " + highestUnlockedLevel);
        PlayerPrefs.SetInt("SelectedFFLevel", highestUnlockedLevel);
        PlayerPrefs.SetString("SelectedMode", "FriendlyFire");
        SceneManager.LoadScene(1);
    }
    public void TurnOnSettingPanel()
    {
        mapSelectionPanel.gameObject.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void TurnOffSettingPanel()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        settingPanel.SetActive(false);
    }
    public void TurnOnShopPanel()
    {
        mapSelectionPanel.gameObject.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void TurnOffShopPanel()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        shopPanel.SetActive(false);
    }
    public int GetCompletedLevels1Count()
    {
        int completedLevels1Count = 0;

        for (int i = 1; i <= 160; i++)
        {
            if (PlayerPrefs.GetInt("Level" + i + "_Win") == 1)
            {
                completedLevels1Count++;
            }
        }

        return completedLevels1Count;
    }
    public int GetCompletedLevels2Count()
    {
        int completedLevels2Count = 0;

        for (int i = 1; i <= 32; i++)
        {
            if (PlayerPrefs.GetInt("HLevel" + i + "_Win") == 1)
            {
                completedLevels2Count++;
            }
        }

        return completedLevels2Count;
    }
    public int GetCompletedLevels3Count()
    {
        int completedLevels3Count = 0;

        for (int i = 1; i <= 32; i++)
        {
            if (PlayerPrefs.GetInt("NLevel" + i + "_Win") == 1)
            {
                completedLevels3Count++;
            }
        }

        return completedLevels3Count;
    }
    public int GetCompletedLevels4Count()
    {
        int completedLevels4Count = 0;

        for (int i = 1; i <= 32; i++)
        {
            if (PlayerPrefs.GetInt("FLevel" + i + "_Win") == 1)
            {
                completedLevels4Count++;
            }
        }

        return completedLevels4Count;
    }
    void ClearPlayerPrefsForLevels()
    {
        if (PlayerPrefs.GetInt("GameUpdated", 0) == 0)
        {
            for (int levelIndex = 64; levelIndex <= 96; levelIndex++)
            {
                PlayerPrefs.DeleteKey("Lv" + levelIndex);
                PlayerPrefs.SetInt("Level" + levelIndex + "_Win", 0);
            }
            PlayerPrefs.SetInt("GameUpdated", 1);
            PlayerPrefs.Save();
        }
    }
    void TransferAndDeletePlayerPrefs()
    {
        if (PlayerPrefs.GetInt("DataTransferred", 0) == 0)
        {
            for (int oldLevelIndex = 65, newLevelIndex = 1; oldLevelIndex <= 96; oldLevelIndex++, newLevelIndex++)
            {
                int stars = PlayerPrefs.GetInt("Lv" + oldLevelIndex, 0);
                PlayerPrefs.SetInt("HLv" + newLevelIndex, stars);

                int winStatus = PlayerPrefs.GetInt("Level" + oldLevelIndex + "_Win", 0);
                PlayerPrefs.SetInt("HLevel" + newLevelIndex + "_Win", winStatus);

                PlayerPrefs.DeleteKey("Lv" + oldLevelIndex);
                PlayerPrefs.DeleteKey("Level" + oldLevelIndex + "_Win");
            }

            PlayerPrefs.SetInt("DataTransferred", 1);
            PlayerPrefs.Save();
        }
    }


}
