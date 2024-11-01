using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public bool unlocked;

    public Image lockImage;
    public GameObject[] stars;
    public Sprite starSprite;
    public TextMeshProUGUI levelText;
    private int levelId;
    
    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(PressSelection);
        }
        //PlayerPrefs.DeleteAll();
        levelText = GetComponentInChildren<TextMeshProUGUI>();
        levelId = int.Parse(gameObject.name);
        levelText.text = levelId.ToString();
    }

    private void Update()
    {
        //UpdateLevelImage();
        //UpdateLevelStatus();
    }
    public void UpdateLevelStatus()
    {
        //if current lv is 5, the pre should be 4
        int previousLevelNum = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("Lv" + previousLevelNum.ToString()) >= 0 &&
        PlayerPrefs.GetInt("Level" + previousLevelNum.ToString() + "_Win") == 1)//If star >= 0 and win, next level can play
        {
            unlocked = true;
        }
    }
    public void UpdateLevelImage()
    {
        if (!unlocked) // level lock
        {
            lockImage.gameObject.SetActive(true);
            for(int i = 0;i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else //level open
        {
            lockImage.gameObject.SetActive(false);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
            //int starCount = Mathf.Min(PlayerPrefs.GetInt("Lv" + gameObject.name), stars.Length);
            for (int i = 0; i < PlayerPrefs.GetInt("Lv" + gameObject.name); i++)
            {
                stars[i].gameObject.GetComponent<Image>().sprite = starSprite;
            }
        }
    }
    public void PressSelection()
    {
        Debug.Log("Button Pressed!");
        Debug.Log(unlocked);
        if (unlocked)
        {
            PlayerPrefs.SetInt("SelectedLevel", levelId);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Level is locked, cannot load scene.");
        }
    }
    public void PressHostageSelection(int levelHId)
    {
        Debug.Log("Button Pressed!");
        Debug.Log(unlocked);
        if (unlocked)
        {
            PlayerPrefs.SetInt("SelectedLevel", levelHId);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Level is locked, cannot load scene.");
        }
    }

}
