using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting : MonoBehaviour
{
    public Button[] buttons; 
    public TextMeshProUGUI[] buttonTexts; 
    private bool[] buttonStates;

    void Start()
    {
        buttonStates = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            buttonStates[i] = PlayerPrefs.GetInt("ButtonState_" + index, 1) == 1;

            buttons[i].onClick.AddListener(() => ToggleButton(index));
        }

        UpdateAllButtonTexts();
    }

    void ToggleButton(int index)
    {
        buttonStates[index] = !buttonStates[index];

        PlayerPrefs.SetInt("ButtonState_" + index, buttonStates[index] ? 1 : 0);
        PlayerPrefs.Save();

        UpdateAllButtonTexts();
    }

    void UpdateAllButtonTexts()
    {
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i].text = $"{buttonTexts[i].name}: " + (buttonStates[i] ? "On" : "Off");
        }
    }
}
