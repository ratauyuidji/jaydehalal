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
    public int stars;
    public MapSelection[] mapSelection;
    public TextMeshProUGUI[] questStarsText;
    public TextMeshProUGUI[] lockedStarsText;
    public TextMeshProUGUI[] unlockedStarsText;
    public TextMeshProUGUI startText;

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
                    unlockedStarsText[i].text = (PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3)) + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    break;
                case 1:
                    unlockedStarsText[i].text = (PlayerPrefs.GetInt("Lv" +4) + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6)) + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    break;
                case 2:
                    unlockedStarsText[i].text = (PlayerPrefs.GetInt("Lv" + 7) + PlayerPrefs.GetInt("Lv" + 8)) + "/" + (mapSelection[i].endLevel - mapSelection[i].startLevel + 1) * 3;
                    break;
            }
        }
    }
    private void UpdateStarUI()
    {
        stars = 0;
        for (int i = 1; i <= 8; i++)
        {
            stars += PlayerPrefs.GetInt("Lv" + i);
        }
        startText.text = stars.ToString() + "/" + 24;
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
