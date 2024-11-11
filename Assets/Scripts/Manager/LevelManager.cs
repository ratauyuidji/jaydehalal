using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<IconHandler> iconHandler;
    public ChapterPanelManager chapterPanelManager;
    public GameObject[] levelPrefabs;
    public GameObject[] hostageLevelPrefabs;
    public GameObject[] nadeLevelPrefabs;
    private GameObject currentLevelPrefab;
    private int levelIndex;
    private int levelHostageIndex;
    private int levelNadeIndex;
    public Button addAmmoButton;
    public Button addNadeButton;
    public Button switchBazookaButton;
    public Button switchNadeButton;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI nadeText;
    public int[] maxNumberOfShoots;
    public int[] hostageMaxNumberOfShoots;
    public int[] nadeMaxNumberOfShoots;
    public IconHandler activeIconHandler;
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
        if (PlayerPrefs.HasKey("SelectedMode"))
        {
            string selectedMode = PlayerPrefs.GetString("SelectedMode");
            Debug.Log("SelectedMode: " + selectedMode);
            if (selectedMode == "Classic" && PlayerPrefs.HasKey("SelectedLevel"))
            {
                levelIndex = PlayerPrefs.GetInt("SelectedLevel");
                Debug.Log("Level Index đã chọn từ menu: " + levelIndex);
                LoadLevel(levelIndex - 1);
            }
            else if (selectedMode == "Hostage" && PlayerPrefs.HasKey("SelectedHostageLevel"))
            {
                levelHostageIndex = PlayerPrefs.GetInt("SelectedHostageLevel");
                Debug.Log("Hostage Level Index đã chọn từ menu: " + levelHostageIndex);
                LoadHostageLevel(levelHostageIndex - 1);
            }
            else if (selectedMode == "Nade" && PlayerPrefs.HasKey("SelectedNadeLevel"))
            {
                levelNadeIndex = PlayerPrefs.GetInt("SelectedNadeLevel");
                Debug.Log("Hostage Level Index đã chọn từ menu: " + levelNadeIndex);
                LoadNadeLevel(levelNadeIndex - 1);
            }
            else
            {
                Debug.LogError("Không tìm thấy level đã chọn!");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy chế độ đã chọn!");
        }
    }
    public void LoadLevel(int index)
    {
        SetActiveIconHandler(0);
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
            activeIconHandler.SetMaxNumberOfShoot(4);
            GameManager.Instance.maxNumberOfShoot = 4;
            Debug.Log("using test level prefab with bullet = 4");
        }
        else if (index >= 0 && index < levelPrefabs.Length)
        {
            currentLevelPrefab = Instantiate(levelPrefabs[index]);
            levelIndex = index + 1;
            chapterPanelManager.Initialize(levelIndex);
            Debug.Log("Level Index from prefab name: " + levelIndex);
            Debug.Log("ten prefab" + currentLevelPrefab.name);
            activeIconHandler.SetMaxNumberOfShoot(maxNumberOfShoots[index]);
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
    public void LoadHostageLevel(int index)
    {
        SetActiveIconHandler(0);
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            {
                Destroy(bullet);
            }
        }
        StartCoroutine(WaitAndLoadHostageLevel(index));
    }

    private IEnumerator WaitAndLoadHostageLevel(int index)
    {
        yield return new WaitForEndOfFrame();
        if (testLevelPrefab != null)
        {
            currentLevelPrefab = Instantiate(testLevelPrefab);
            levelIndex = 0;
            activeIconHandler.SetMaxNumberOfShoot(4);
            GameManager.Instance.maxNumberOfShoot = 4;
            Debug.Log("using test level prefab with bullet = 4");
        }
        else if (index >= 0 && index < hostageLevelPrefabs.Length)
        {
            currentLevelPrefab = Instantiate(hostageLevelPrefabs[index]);
            levelHostageIndex = index + 1;
            chapterPanelManager.Initialize(levelHostageIndex);
            Debug.Log("Hostage Level Index from prefab name: " + levelHostageIndex);
            Debug.Log("Tên prefab: " + currentLevelPrefab.name);
            activeIconHandler.SetMaxNumberOfShoot(hostageMaxNumberOfShoots[index]);
            GameManager.Instance.maxNumberOfShoot = hostageMaxNumberOfShoots[index];
            Debug.Log("Số đạn: " + GameManager.Instance.maxNumberOfShoot);
        }
        else
        {
            Debug.LogError("Index out of bounds for hostage levels!");
        }
        GetButton();
        GameManager.Instance.SetUpWhenLoadLevel();
    }
    public void LoadNadeLevel(int index)
    {
        SetActiveIconHandler(1);
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
            foreach (GameObject nade in GameObject.FindGameObjectsWithTag("Nade"))
            {
                Destroy(nade);
            }
        }
        StartCoroutine(WaitAndLoadNadeLevel(index));
    }

    private IEnumerator WaitAndLoadNadeLevel(int index)
    {
        yield return new WaitForEndOfFrame();
        if (testLevelPrefab != null)
        {
            currentLevelPrefab = Instantiate(testLevelPrefab);
            levelIndex = 0;
            activeIconHandler.SetMaxNumberOfShoot(4);
            GameManager.Instance.maxNumberOfShoot = 4;
            Debug.Log("using test level prefab with bullet = 4");
        }
        else if (index >= 0 && index < nadeLevelPrefabs.Length)
        {
            currentLevelPrefab = Instantiate(nadeLevelPrefabs[index]);
            levelNadeIndex = index + 1;
            chapterPanelManager.Initialize(levelNadeIndex);
            Debug.Log("Nade Level Index from prefab name: " + levelNadeIndex);
            Debug.Log("Tên prefab: " + currentLevelPrefab.name);
            activeIconHandler.SetMaxNumberOfShoot(nadeMaxNumberOfShoots[index]);
            GameManager.Instance.maxNumberOfShoot = nadeMaxNumberOfShoots[index];
            Debug.Log("Số đạn: " + GameManager.Instance.maxNumberOfShoot);
        }
        else
        {
            Debug.LogError("Index out of bounds for hostage levels!");
        }
        GetButton();
        GameManager.Instance.SetUpWhenLoadLevel();
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
    public int GetHostageLevelIndex()
    {
        return levelHostageIndex;
    }
    public int GetNadeLevelIndex()
    {
        return levelNadeIndex;
    }
    private void SetActiveIconHandler(int index)
    {
        for (int i = 0; i < iconHandler.Count; i++)
        {
            iconHandler[i].gameObject.SetActive(i == index);
        }
        activeIconHandler = iconHandler[index];
    }
}
