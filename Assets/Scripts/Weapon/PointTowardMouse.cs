using UnityEngine;

public class PointTowardMouse : MonoBehaviour
{
    private GameObject targetObject;
    private SpriteRenderer targetSpriteRenderer; 

    Vector2 MousePos
    {
        get
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
            return pos;
        }
    }

    private void Start()
    {
        targetObject = GameObject.Find("icon_target");
        if (targetObject != null)
        {
            targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
            targetObject.SetActive(false); 
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

            if (targetObject != null)
            {
                targetObject.SetActive(true); 
                MoveObjectToMousePosition();
            }
        }
        else
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false); 
            }
        }
    }

    private void MoveObjectToMousePosition()
    {
        if (targetSpriteRenderer != null)
        {
            targetObject.transform.position = MousePos;
        }
    }
}
