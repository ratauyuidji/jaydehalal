using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointTowardMouse : MonoBehaviour
{
    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return pos;
        }
    }
    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 dir = (Vector2)transform.position - MousePos;
            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.eulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
    }
}
