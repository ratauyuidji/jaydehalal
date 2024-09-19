using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI completedLevelsMap1Text;
    [SerializeField] private TextMeshProUGUI completedLevelsMap2Text;
    [SerializeField] private TextMeshProUGUI starMap1Text;
    [SerializeField] private TextMeshProUGUI starMap2Text;

    public static UIManager Instance;
    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanels;
    public GameObject settingPanel;
    public int stars;
    public MapSelection[] mapSelection;
    public TextMeshProUGUI[] questStarsText;
    public TextMeshProUGUI[] lockedStarsText;
    public TextMeshProUGUI[] unlockedStarsText;
    public TextMeshProUGUI startText;
    public Image[] checkmarks;
    private Vector3[] initialPositions;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonTexts;
    private int starMap1;
    private int starMap2;



    private void Awake()
    {
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
        buttonStates = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ToggleButton(index));
            buttonStates[i] = true;
        }
        UpdateAllButtonTexts();
    }
    void ToggleButton(int index)
    {
        buttonStates[index] = !buttonStates[index];
        UpdateAllButtonTexts();
    }
    void UpdateAllButtonTexts()
    {
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i].text = $"{buttonTexts[i].name}: " + (buttonStates[i] ? "On" : "Off");
        }
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

        completedLevelsMap1Text.text = completedLevels1Count.ToString() + "/" + "36";
        completedLevelsMap2Text.text = completedLevels2Count.ToString() + "/" + "24";
        starMap1Text.text = starMap1.ToString();
        starMap2Text.text = starMap2.ToString();
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
            unlockedStarsText[i].text = stars.ToString() + "/" + mapSelection[i].endLevel * 3;
            switch (i)
            {
                case 0:
                    int totalStars0 = 0;
                    for (int lv = 1; lv <= 36; lv++)
                    {
                        totalStars0 += PlayerPrefs.GetInt("Lv" + lv);
                    }
                    int maxStars = (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    unlockedStarsText[i].text = totalStars0 + "/" + maxStars;
                    starMap1 = totalStars0;
                    break;

                case 1:
                    int totalStars1 = 0;
                    for (int lv = 37; lv <= 60; lv++)
                    {
                        totalStars1 += PlayerPrefs.GetInt("Lv" + lv);
                    }
                    unlockedStarsText[i].text = totalStars1 + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    starMap2 = totalStars1;
                    break;
                case 2:
                    unlockedStarsText[i].text = 0 + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    break;
            }
        }
    }
    public void UpdateStarUI()
    {
        stars = 0;
        for (int i = 1; i <= 60; i++)
        {
            stars += PlayerPrefs.GetInt("Lv" + i);
        }
        startText.text = stars.ToString() + "/" + 180;
    }
    public void PressMapButton(int mapIndex)
    {
        if (mapSelection[mapIndex].isUnlock == true)
        {
            mapSelectionPanel.gameObject.SetActive(false);
            StartCoroutine(ShowPanel(levelSelectionPanels[mapIndex], mapIndex));

            LevelSelection[] levels = levelSelectionPanels[mapIndex].GetComponentsInChildren<LevelSelection>();
            foreach (LevelSelection level in levels)
            {
                level.UpdateLevelStatus();
                level.UpdateLevelImage();
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
        for(int i = 0;i < mapSelection.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }
    public void LoadHighestUnlockedLevelMode1()
    {
        LoadHighestUnlockedLevel(1, 35);
    }

    public void LoadHighestUnlockedLevelMode2()
    {
        LoadHighestUnlockedLevel(37, 59);
    }

    private void LoadHighestUnlockedLevel(int start, int end)
    {
        int highestUnlockedLevel = start;

        for (int i = start; i <= end; i++)
        {
            if (PlayerPrefs.GetInt("Lv" + i) >= 0 && PlayerPrefs.GetInt("Level" + i + "_Win") == 1)
            {
                highestUnlockedLevel = i + 1;
            }
            else
            {
                break; // stop if level lock
            }
        }

        string levelName = "Level" + highestUnlockedLevel;
        Debug.Log("Loading highest unlocked level: " + levelName);
        SceneManager.LoadScene(levelName);
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
    public int GetCompletedLevels1Count()
    {
        int completedLevels1Count = 0;

        for (int i = 1; i <= 36; i++)
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

        for (int i = 37; i <= 60; i++)
        {
            if (PlayerPrefs.GetInt("Level" + i + "_Win") == 1)
            {
                completedLevels2Count++;
            }
        }

        return completedLevels2Count;
    }
}
