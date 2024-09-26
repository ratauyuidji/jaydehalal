using UnityEngine;

public class ChangeMultipleSprites : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpriteIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSprite();
        }
    }

    void ChangeSprite()
    {
        if (sprites.Length > 0)
        {
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
            PlayerPrefs.SetInt("SelectedSkin", currentSpriteIndex);
        }
    }
}
