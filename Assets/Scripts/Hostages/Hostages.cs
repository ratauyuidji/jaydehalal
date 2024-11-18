using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostages : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] public float damageThreshold = 1f;
    public GameObject head;
    public Sprite deadHeadSprite;
    private float currentHealth;
    public bool isDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDied = false;
    }



    public void TakeDamage(float damageAmount)
    {
        if (isDied) return;
        Debug.Log($"TakeDamage called with damageAmount: {damageAmount}");
        currentHealth -= damageAmount;
        Debug.Log($"Current Health: {currentHealth}");
        if (currentHealth <= 0)
        {
            ActivateDeadSprite();
            isDied = true;
            GameManager.Instance.LoseGame();
        }
    }
    public void ActivateDeadSprite()
    {
        head.gameObject.GetComponent<SpriteRenderer>().sprite = deadHeadSprite;
    }
    public void DisableChildrenHingeLimits()
    {
        HostagesChild[] hostageChildren = GetComponentsInChildren<HostagesChild>();
        foreach (var child in hostageChildren)
        {
            child.DisableHingeLimit();
        }
    }
}
