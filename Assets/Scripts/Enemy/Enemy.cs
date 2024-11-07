using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
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
    private void Start()
    {
        
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
            Debug.Log("Enemy Died");
            GameManager.Instance.RemoveEnemy(this);
        }
    }
    public void OnBulletHit(Collider2D other)
    {
        ActivateDeadSprite();
        DisableChildrenHingeLimits();
        GameManager.Instance.RemoveEnemy(this);
    }
    public void ActivateDeadSprite()
    {
        head.gameObject.GetComponent<SpriteRenderer>().sprite = deadHeadSprite;
    }
    public void DisableChildrenHingeLimits()
    {
        EnemyChildren[] enemyChildren = GetComponentsInChildren<EnemyChildren>();
        foreach (var child in enemyChildren)
        {
            child.DisableHingeLimit();
        }
    }
}
