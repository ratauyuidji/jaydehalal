using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostagesChild : MonoBehaviour
{
    [SerializeField] private GameObject deathVFXPrefab;
    private bool isDeathVFXEnabled = true;
    private Hostages parentHostages;
    private HingeJoint2D hingeJoint2d;
    private Rigidbody2D rb;

    private void Start()
    {
        hingeJoint2d = GetComponent<HingeJoint2D>();
        bool isVFXEnabled = PlayerPrefs.GetInt("ButtonState_3", 1) == 1;
        ToggleDeathVFX(isVFXEnabled);
        parentHostages = GetComponentInParent<Hostages>();
        rb = GetComponent<Rigidbody2D>();
        //rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("CanDestroyBox") || other.gameObject.CompareTag("Wall"))
        {
            float impactVelocity = other.relativeVelocity.magnitude;
            if (impactVelocity > parentHostages.damageThreshold)
            {
                parentHostages.DisableChildrenHingeLimits();
                if (isDeathVFXEnabled)
                {
                    Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
                }
            }
            if (impactVelocity > parentHostages.damageThreshold && !parentHostages.isDied)
            {
                Debug.Log(impactVelocity);
                parentHostages.TakeDamage(impactVelocity);
                if (isDeathVFXEnabled)
                {
                    Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            parentHostages.ActivateDeadSprite();
            if (isDeathVFXEnabled)
            {
                Instantiate(deathVFXPrefab, this.transform.position, Quaternion.identity);
            }
        }
    }
    public void ToggleDeathVFX(bool isEnabled)
    {
        isDeathVFXEnabled = isEnabled;
    }
    public void DisableHingeLimit()
    {
        if (hingeJoint2d != null)
        {
            hingeJoint2d.useLimits = false;
            rb.isKinematic = false;
        }
    }
}
