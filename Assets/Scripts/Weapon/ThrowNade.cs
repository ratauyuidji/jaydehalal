using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ThrowNade : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private TextMeshProUGUI ammoText;
    private ChangeWeapon changeWeapon;

    public GameObject nadePrefab;
    public Transform throwPoint;
    public float baseThrowForce = 20f;
    private float throwForce;
    private int currentNadeNumber;
    private bool canThrow = true;

    private void Awake()
    {
        currentNadeNumber = 1;
        UpdateNadeNumberText();
        changeWeapon = FindObjectOfType<ChangeWeapon>();
    }
    private void OnEnable()
    {
        if (currentNadeNumber > 0)
        {
            canThrow = true;
        }
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonUp(0) && canThrow)
        {
            if(currentNadeNumber > 0)
            {
                lineRenderer.enabled = false;
                ThrowGrenade();
                currentNadeNumber--;
                UpdateNadeNumberText();
                if (currentNadeNumber == 0)
                {
                    canThrow = false;
                }
            }
        }
        if (Mouse.current.leftButton.isPressed && canThrow)
        {
            lineRenderer.enabled = true;
            DrawLine();
        }
    }

    void ThrowGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwDirection = (mousePos - (Vector2)throwPoint.position).normalized;
        GameObject nade = Instantiate(nadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody2D rb = nade.GetComponent<Rigidbody2D>();

        throwForce = CalculateThrowForce();
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        StartCoroutine(ExplodeAfterDelay(nade, 3f));

        /*if (changeWeapon.currentWeaponIndex == 2)
        {
            changeWeapon.SwitchToFirstWeapon();
        }*/
    }
    private IEnumerator ExplodeAfterDelay(GameObject nade, float delay)
    {
        yield return new WaitForSeconds(delay);
        nade.GetComponent<Nade>().Explode();
    }

    private void DrawLine()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        SetLine(touchPosition);
    }

    private void SetLine(Vector2 position)
    {
        Vector2 direction = position - (Vector2)startPosition.position;
        float distance = Mathf.Min(direction.magnitude, maxDistance);

        Vector2 clampedPosition = (Vector2)startPosition.position + direction.normalized * distance;

        lineRenderer.SetPosition(1, clampedPosition);
        lineRenderer.SetPosition(0, startPosition.position);
    }

    private float CalculateThrowForce()
    {
        float lineLength = Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        float normalizedLength = lineLength / maxDistance;
        return baseThrowForce * normalizedLength;
    }
    public void AddNade(int ammo)
    {
        currentNadeNumber = Mathf.Clamp(currentNadeNumber + ammo, 0, maxAmmo);
        if (currentNadeNumber > 0)
        {
            canThrow = true;
        }
        UpdateNadeNumberText();
    }
    private void UpdateNadeNumberText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentNadeNumber.ToString();
        }
    }
}
