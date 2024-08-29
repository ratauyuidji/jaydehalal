using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserPos;
    public Transform target; 
 

    Vector2 TargetPos
    {
        get
        {
            if (target != null)
            {
                return target.position;
            }
            return Vector2.zero;
        }
    }

    private void Update()
    {
        if (!enabled) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            lineRenderer.enabled = false;
            return;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            lineRenderer.enabled = true;
            Vector2 dir = TargetPos - (Vector2)laserPos.transform.position;
            Vector2 startLaserPos = (Vector2)laserPos.transform.position + dir.normalized;

            RaycastHit2D hit = Physics2D.Raycast(startLaserPos, dir, 50f);

            lineRenderer.SetPosition(0, startLaserPos);

            if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("FallBox") || hit.collider.CompareTag("Joint")))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, startLaserPos + dir.normalized * 50f);
            }
        }
    }

    public void DeactivateLaser()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            lineRenderer.enabled = false;
        }
    }
}
