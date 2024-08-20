using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    public Transform pressedPosition;
    public float pressSpeed = 1.0f;
    private bool isPressed = false;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        isPressed = true;
    }

    void Update()
    {
        if (isPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, pressedPosition.position, pressSpeed * Time.deltaTime);

            if (transform.position == pressedPosition.position)
            {
                //transform.position = pressedPosition.position;
                isPressed = false;
            }
        }
    }
}

