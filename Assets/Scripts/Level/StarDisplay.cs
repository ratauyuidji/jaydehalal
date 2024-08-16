using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StarDisplay : MonoBehaviour
{
    public GameObject[] stars;
    public Sprite starSprite;

    public void DisplayStar(int starsNum)
    {
        for (int i = 0; i < starsNum; i++)
        {
            stars[i].gameObject.GetComponent<Image>().sprite = starSprite;
        }
    }
}
