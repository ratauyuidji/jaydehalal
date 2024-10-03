using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Hapiga.Ads;

public class ThrowNade : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private TextMeshProUGUI ammoText;
    private ChangeWeapon changeWeapon;
    public RotateArm rotateArm;
    public GameObject nadePrefab;
    public Transform throwPoint;
    public float baseThrowForce = 20f;
    private float throwForce;
    private int currentNadeNumber;
    private bool canThrow = true;

    private const string NadeKey = "CurrentNadeNumber";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(NadeKey))
        {
            currentNadeNumber = PlayerPrefs.GetInt(NadeKey);
        }
        else
        {
            currentNadeNumber = 0;
        }

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
        if (TouchUI.IsPointerOverUI())
            return;

        bool isMouseOverArmWithGun = IsMouseOverArmWithGun();

        if (isMouseOverArmWithGun)
        {
            lineRenderer.enabled = false;
        }
        else if (canThrow)
        {
            if (InputManager.wasLeftMouseButtonReleased && GameManager.Instance.RaycastForCanFire())
            {
                if (currentNadeNumber > 0)
                {
                    ThrowGrenade();
                    currentNadeNumber--;
                    UpdateNadeNumberText();
                    SaveNadeNumber();
                    if (currentNadeNumber == 0)
                    {
                        canThrow = false;
                    }
                }
                lineRenderer.enabled = false;
            }

            if (InputManager.IsLeftMousePressed)
            {
                lineRenderer.enabled = true;
                DrawLine();
            }
        }
    }

    void ThrowGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        Vector2 throwDirection = (mousePos - (Vector2)throwPoint.position).normalized;
        GameObject nade = Instantiate(nadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody2D rb = nade.GetComponent<Rigidbody2D>();

        throwForce = CalculateThrowForce();
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        if (rotateArm != null)
        {
            Debug.Log("Calling ArmRotate");
            rotateArm.ArmRotate();
        }
        if (changeWeapon.currentWeaponIndex == 2)
        {
            changeWeapon.SwitchToFirstWeapon();
        }
    }

    private void DrawLine()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        Vector3 direction = (startPosition.position - touchPosition).normalized;
        touchPosition += direction * 7f;
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
        AdManager.Instance.ShowRewardedVideo(CloseRewardCallback);
    }
    void CloseRewardCallback()
    {
        currentNadeNumber = Mathf.Clamp(currentNadeNumber + 1, 0, maxAmmo);
        if (currentNadeNumber > 0)
        {
            canThrow = true;
        }
        UpdateNadeNumberText();
        SaveNadeNumber();
        GameManager.Instance.TurnOffAddPanel();
    }

    private void UpdateNadeNumberText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentNadeNumber.ToString();
        }
    }

    private void SaveNadeNumber()
    {
        PlayerPrefs.SetInt(NadeKey, currentNadeNumber);
        PlayerPrefs.Save();
    }

    private bool IsMouseOverArmWithGun()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            Debug.Log("Hit detected on: " + hit.collider.name);
            Transform parent = hit.collider.transform;
            foreach (Transform child in parent.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("ArmWithGun"))
                {
                    Debug.Log("Mouse is over ArmWithGun");
                    return true;
                }
            }
        }
        return false;
    }
}
