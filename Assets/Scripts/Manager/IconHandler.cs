using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private Color usedColor;

    private Color[] originalColors;
    private int maxNumberOfShoot;

    private void Start()
    {
        originalColors = new Color[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            originalColors[i] = icons[i].color;
        }
    }

    public void SetMaxNumberOfShoot(int maxShoot)
    {
        maxNumberOfShoot = maxShoot;

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(i < maxNumberOfShoot); // Tắt các icon không sử dụng
        }
    }

    public void UseShot(int shotNumber)
    {
        if (shotNumber > 0 && shotNumber <= maxNumberOfShoot)
        {
            int index = maxNumberOfShoot - shotNumber;
            icons[index].color = usedColor;
        }
    }

    public void PlusShot(int shotNumber)
    {
        if (shotNumber >= 0 && shotNumber < maxNumberOfShoot)
        {
            int index = maxNumberOfShoot - shotNumber - 1;
            icons[index].color = originalColors[index];
        }
    }
    public void ResetIcons()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].color = originalColors[i];
            icons[i].gameObject.SetActive(i < maxNumberOfShoot);
        }
    }

}
