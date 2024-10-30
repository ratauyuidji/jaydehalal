using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public IconHandler iconHandler;
    public ChapterPanelManager chapterPanelManager;
    public GameObject[] levelPrefabs;
    private GameObject currentLevelPrefab;
    private int levelIndex;
    public Button addAmmoButton;
    public Button addNadeButton;
    public Button switchBazookaButton;
    public Button switchNadeButton;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI nadeText;
    public int[] maxNumberOfShoots;

    public GameObject testLevelPrefab;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("SelectedLevel"))
        {
            levelIndex = PlayerPrefs.GetInt("SelectedLevel");
            Debug.Log("levelindex đã chọn từ menu" + levelIndex);
            LoadLevel(levelIndex-1);
        }
        else
        {
            Debug.LogError("Không tìm thấy level đã chọn!");
        }
    }

    public void LoadLevel(int index)
    {
        
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            {
                Destroy(bullet);
            }
        }
        StartCoroutine(WaitAndLoadLevel(index));
    }
    private IEnumerator WaitAndLoadLevel(int index)
    {
        yield return new WaitForEndOfFrame();
        if (testLevelPrefab != null)
        {
            currentLevelPrefab = Instantiate(testLevelPrefab);
            levelIndex = 0;
            iconHandler.SetMaxNumberOfShoot(4);
            GameManager.Instance.maxNumberOfShoot = 4;
            Debug.Log("Đang sử dụng test level prefab với số đạn tối đa: 4");
        }
        else if (index >= 0 && index < levelPrefabs.Length)
        {
            currentLevelPrefab = Instantiate(levelPrefabs[index]);
            levelIndex = index + 1;
            chapterPanelManager.Initialize(levelIndex);
            Debug.Log("Level Index from prefab name: " + levelIndex);
            Debug.Log("ten prefab" + currentLevelPrefab.name);
            iconHandler.SetMaxNumberOfShoot(maxNumberOfShoots[index]);
            GameManager.Instance.maxNumberOfShoot = maxNumberOfShoots[index];
            Debug.Log("so dan" + GameManager.Instance.maxNumberOfShoot);
        }
        else
        {
            Debug.LogError("Index out of bounds!");
        }
        GetButton();
        GameManager.Instance.SetUpWhenLoadLevel();
        
    }

    private int ExtractLevelIndex(string prefabName)
    {
        if (prefabName.Contains("(Clone)"))
        {
            prefabName = prefabName.Replace("(Clone)", "").Trim();
        }
        for (int i = 0; i < prefabName.Length; i++)
        {
            if (char.IsDigit(prefabName[i]))
            {
                string numberPart = prefabName.Substring(i);
                if (int.TryParse(numberPart, out int levelIndex))
                {
                    return levelIndex;
                }
            }
        }
        Debug.LogError("Không tìm thấy số trong tên prefab: " + prefabName);
        return 0;
    }


    private void GetButton()
    {
        var player = GameObject.FindWithTag("Player");
        var gun = player.transform.Find("ArmWithGun/Gun (1)").GetComponent<Bazooka>();
        var nade = player.transform.Find("ArmWithGun/Nade").GetComponent<ThrowNade>();
        var arm = player.transform.Find("ArmWithGun").GetComponent<ChangeWeapon>();

        if (gun != null && ammoText != null && nade != null && nadeText != null)
        {
            gun.SetAmmoText(ammoText);
            nade.SetNadeText(nadeText);
        }

        addAmmoButton.onClick.RemoveAllListeners();
        addAmmoButton.onClick.AddListener(gun.AddAmmo);

        addNadeButton.onClick.RemoveAllListeners();
        addNadeButton.onClick.AddListener(nade.AddNade);

        switchBazookaButton.onClick.RemoveAllListeners();
        switchBazookaButton.onClick.AddListener(arm.SwitchToGun2);

        switchNadeButton.onClick.RemoveAllListeners();
        switchNadeButton.onClick.AddListener(arm.SwitchToNade);
    }
    public int GetLevelIndex()
    {
        return levelIndex;
    }
}
