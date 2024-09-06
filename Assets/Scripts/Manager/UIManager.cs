using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
    public Button[] buttons;
    public Image[] checkmarks;
    private bool[] states;
    private Vector3[] initialPositions;



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
    void Start()
    {
        states = new bool[buttons.Length];

        for (int i = 0; i < states.Length; i++)
        {
            states[i] = true;
            int index = i;
            buttons[i].onClick.AddListener(() => Toggle(index));
        }
    }
    void Toggle(int index)
    {
        states[index] = !states[index];
        checkmarks[index].gameObject.SetActive(states[index]);
    }
    private void Update()
    {
        UpdateLockedStarUI();
        UpdateUnlockedStarUI();
        UpdateStarUI();
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
                    int totalStars = 0;
                    for (int lv = 1; lv <= 32; lv++)
                    {
                        totalStars += PlayerPrefs.GetInt("Lv" + lv);
                    }
                    int maxStars = (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    unlockedStarsText[i].text = totalStars + "/" + maxStars;
                    break;

                case 1:
                    unlockedStarsText[i].text = 0 + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
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
        for (int i = 1; i <= 32; i++)
        {
            stars += PlayerPrefs.GetInt("Lv" + i);
        }
        startText.text = stars.ToString() + "/" + 96;
    }
    public void PressMapButton(int mapIndex)
    {
        if (mapSelection[mapIndex].isUnlock == true)
        {
            mapSelectionPanel.gameObject.SetActive(false);
            StartCoroutine(ShowPanel(levelSelectionPanels[mapIndex], mapIndex));

            // Get all LevelSelection components in the map's level selection panel and update their status/images
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
    public void LoadHighestUnlockedLevel()
    {
        int highestUnlockedLevel = 1; 

        for (int i = 1; i <= 31; i++) // 3 level
        {
            if (PlayerPrefs.GetInt("Lv" + i) > 0)
            {
                highestUnlockedLevel = i +1;
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
        settingPanel.SetActive(true);
    }
    public void TurnOffSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
