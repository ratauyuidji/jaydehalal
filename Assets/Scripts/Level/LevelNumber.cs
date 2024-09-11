using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNumber : MonoBehaviour
{
    public TextMeshProUGUI levelNumber;

    private void Start()
    {
        CurrentLevelNumber();
    }

    private void CurrentLevelNumber()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentLevelIndex > 32)
        {
            currentLevelIndex -= 32;
        }

        levelNumber.text = "Level " + currentLevelIndex.ToString();
    }
}
