using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostages : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] public float damageThreshold = 1f;
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
            isDied = true;
            GameManager.Instance.LoseGame();
        }
    }
}
