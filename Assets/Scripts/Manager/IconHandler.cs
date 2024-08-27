using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private Color usedColor;

    private Color[] originalColors;

    private void Start()
    {
        originalColors = new Color[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            originalColors[i] = icons[i].color;
        }
    }

    public void UseShot(int shotNumber)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (shotNumber == i + 1) // icon from 0, shot from 1
            {
                icons[i].color = usedColor;
                return;
            }
        }
    }

    public void PlusShot(int shotNumber)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (shotNumber == i) // icon from 0, shot from 1
            {
                icons[i].color = originalColors[i];
                return;
            }
        }
    }
}
