using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI starsText;

    private void Update()
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
    }
}
