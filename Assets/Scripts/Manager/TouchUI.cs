using UnityEngine;
using UnityEngine.EventSystems;

public static class TouchUI
{
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("mouse");
            return true;
        }
        if (Input.touches.Length > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                return true;
            }
        }       

        return false;
    }
}
