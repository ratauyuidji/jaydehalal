using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardMouse : MonoBehaviour
{
    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return pos;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 dir = (Vector2)transform.position - MousePos;
            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.eulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
    }
}
