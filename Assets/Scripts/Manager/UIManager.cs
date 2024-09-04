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
    private void UpdateLockedStarUI()
    {
        for(int i = 0;i < mapSelection.Length;i++)
        {
            questStarsText[i].text = mapSelection[i].questNum.ToString();
            if (mapSelection[i].isUnlock == false)
            {
                lockedStarsText[i].text = stars.ToString() + "/" + mapSelection[i].endLevel * 3;
            }
        }

    }
    private void UpdateUnlockedStarUI()
    {
        for(int i = 0; i < mapSelection.Length; i++)
        {
            unlockedStarsText[i].text = stars.ToString() + "/" + mapSelection[i].endLevel * 3;
            switch (i)
            {
                case 0:
                    int totalStars = 0;
                    for (int lv = 1; lv <= 12; lv++)
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
    private void UpdateStarUI()
    {
        stars = 0;
        for (int i = 1; i <= 12; i++)
        {
            stars += PlayerPrefs.GetInt("Lv" + i);
        }
        startText.text = stars.ToString() + "/" + 36;
    }
    public void PressMapButton(int mapIndex)
    {
        if (mapSelection[mapIndex].isUnlock == true)
        {
            levelSelectionPanels[mapIndex].gameObject.SetActive(true);
            mapSelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("You can not open this map now");
        }
    }
    public void BackMapButton()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        for(int i = 0;i < mapSelection.Length; i++)
        {
            levelSelectionPanels[i].gameObject.SetActive(false);
        }
    }
    public void LoadHighestUnlockedLevel()
    {
        int highestUnlockedLevel = 1; 

        for (int i = 1; i <= 11; i++) // 3 level
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




    //public TextMeshProUGUI starsText;

    /*private void Update()
    {
        UpdateStarsUI();
    }

    public void UpdateStarsUI()
    {
        int sum = 0;

        for (int i = 1; i < 9; i++)
        {
            sum += PlayerPrefs.GetInt("Lv" + i.ToString());//Add level 1,2... stars number

            starsText.text = sum + "/" + 24;
        }
    }*/
}
