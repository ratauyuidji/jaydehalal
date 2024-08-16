using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;

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
        if (!enabled) return;
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            Vector2 dir = MousePos - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 50f);

            lineRenderer.SetPosition(0, transform.position);

            if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("FallBox") || hit.collider.CompareTag("Joint")))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, (Vector2)transform.position + dir.normalized * 50f);
            }
        }

    }
    public void DeactivateLaser()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
        }
    }
}
