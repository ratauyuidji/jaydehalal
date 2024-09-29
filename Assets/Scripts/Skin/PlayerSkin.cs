using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] private SkinManager skinManager;

    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private SpriteRenderer headRenderer;

    private void Start()
    {
        Skin selectedSkin = skinManager.GetSelectedSkin();

        if (selectedSkin != null)
        {
            bodyRenderer.sprite = selectedSkin.Body;
            armRenderer.sprite = selectedSkin.Arm;
            headRenderer.sprite = selectedSkin.Head;
        }
    }
}
