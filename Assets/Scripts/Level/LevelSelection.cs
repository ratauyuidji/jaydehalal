using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public bool unlocked;

    public Image lockImage;
    public GameObject[] stars;
    public Sprite starSprite;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
    }
    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus();
    }
    private void UpdateLevelStatus()
    {
        //if current lv is 5, the pre should be 4
        int previousLevelNum = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("Lv" + previousLevelNum.ToString()) > 0)//If star > 0, next level can play
        {
            unlocked = true;
        }
    }
    private void UpdateLevelImage()
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
    public void PressSelection(int levelId)
    {
        Debug.Log("Button Pressed!");
        Debug.Log(unlocked);
        if(unlocked)
        {
            string levelname = "Level" + (levelId);
            SceneManager.LoadScene(levelname);
        }
        else
        {
            Debug.Log("Level is locked, cannot load scene.");
        }
    }
}
