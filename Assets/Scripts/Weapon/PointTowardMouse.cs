using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointTowardMouse : MonoBehaviour
{
    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
            return pos;
        }
    }
    private void Update()
    {
        if (TouchUI.IsPointerOverUI())
            return;
        if (InputManager.IsLeftMousePressed && GameManager.Instance.RaycastForCanFire())
        {
            Vector2 dir = (Vector2)transform.position - MousePos;
            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.eulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
//#if UNITY_EDITOR

//#else
//        if (Input.touches.Length > 0)//(InputManager.IsLeftMousePressed)
//        {
//            Vector2 dir = (Vector2)transform.position - MousePos;
//            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
//            transform.eulerAngles = new Vector3(0f, 0f, angle - 90f);
//        }
//#endif

    }

}
