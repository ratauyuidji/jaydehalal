using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private bool canDraw = true;

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
        Vector2 dir = MousePos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 50f);

        lineRenderer.SetPosition(0, transform.position);

        if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("FallBox")))
        {
            lineRenderer.SetPosition(1, hit.point); 
        }
        else
        {
            lineRenderer.SetPosition(1, (Vector2)transform.position + dir.normalized * 50f); 
        }
    }
    public void DeactivateLaser()
    {
        lineRenderer.enabled = false;
        StartCoroutine(DeactivateTime());
    }
    private IEnumerator DeactivateTime()
    {
        yield return new WaitForSeconds(2f);
        lineRenderer.enabled = true;
    }
}
