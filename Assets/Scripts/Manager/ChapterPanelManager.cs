using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChapterPanelManager : MonoBehaviour
{
    public CanvasGroup panelCanvasGroup;
    public GameObject panel;
    public float fadeDuration = 1f;

    public RectTransform imageLeft;
    public GameObject imageRightContainer;
    public List<GameObject> rightImageParentsPrefabs;
    public List<GameObject> cityImagePrefabs;
    public Transform rightImageParentContainer;
    public Transform cityImageParentContainer;

    private GameObject currentRightImage;
    private GameObject currentCityImage;

    public float moveDuration = 1f;
    private Vector3 leftImageStartPos;
    private Vector3 rightImageStartPos;
    private Vector3 leftImageEndPos;
    private Vector3 rightImageEndPos;
    public TextMeshProUGUI chapterText;
    public TextMeshProUGUI mapText;
    private int levelIndex;

    private readonly Dictionary<int, (string chapter, string map)> chapterData = new Dictionary<int, (string, string)>
    {
        { 1, ("CHAPTER 1", "BULLET CITY") },
        { 17, ("CHAPTER 2", "SHOGUN'S CASTLE") },
        { 33, ("CHAPTER 3", "GRAVEYARD") },
        { 49, ("CHAPTER 4", "FAR WEST") },
        { 65, ("CHAPTER 5", "FOREST") },
        { 81, ("CHAPTER 6", "FORTRESS") },
        { 97, ("CHAPTER 7", "PREHISTORY") },
        { 113, ("CHAPTER 8", "UNKNOWN PLANET") },
        { 129, ("CHAPTER 9", "PRIVATE SHIP") },
        { 145, ("CHAPTER 10", "CASTLE") },
        { 161, ("CHAPTER 11", "ROMAN") },
        { 177, ("CHAPTER 12", "SNOW FOREST") },
        { 193, ("CHAPTER 13", "BULLET CITY II") },
        { 209, ("CHAPTER 14", "CIRCUS") },
        { 225, ("CHAPTER 15", "PRISON") },
        { 241, ("CHAPTER 1", "BULLET CITY II") },
        { 257, ("CHAPTER 2", "SHOGUN'S CASTLE II") }

    };

    void Start()
    {
        leftImageEndPos = imageLeft.localPosition;
        rightImageEndPos = imageRightContainer.transform.localPosition;

        leftImageStartPos = leftImageEndPos + new Vector3(-500f, 0f, 0f);
        rightImageStartPos = rightImageEndPos + new Vector3(500f, 0f, 0f);

        imageLeft.localPosition = leftImageStartPos;
        imageRightContainer.transform.localPosition = rightImageStartPos;

        panel.SetActive(false);
        panelCanvasGroup.alpha = 0f;
    }

    public void Initialize(int levelIndex)
    {
        this.levelIndex = levelIndex;

        if (chapterData.TryGetValue(levelIndex, out var chapterInfo))
        {
            chapterText.text = chapterInfo.chapter;
            mapText.text = chapterInfo.map;

            GameManager.Instance.canShot = false;
            Debug.Log("Turn off gun");

            panel.SetActive(true);
            panelCanvasGroup.alpha = 1f;

            SetActiveImages(levelIndex);
            StartCoroutine(MoveImages());
        }
        else
        {
            GameManager.Instance.canShot = true;
        }
    }

    private void SetActiveImages(int levelIndex)
    {
        // Xóa hình ảnh cũ
        if (currentRightImage != null)
        {
            Destroy(currentRightImage);
        }

        if (currentCityImage != null)
        {
            Destroy(currentCityImage);
        }

        // Tìm vị trí tương ứng trong danh sách prefab
        int index = chapterData.Keys.ToList().IndexOf(levelIndex);

        if (index >= 0 && index < rightImageParentsPrefabs.Count)
        {
            // Tải prefab hình ảnh phải
            currentRightImage = Instantiate(rightImageParentsPrefabs[index], rightImageParentContainer);
            currentRightImage.transform.localPosition = Vector3.zero;
            currentRightImage.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy prefab hình ảnh phải phù hợp cho levelIndex: " + levelIndex);
        }

        if (index >= 0 && index < cityImagePrefabs.Count)
        {
            // Tải prefab hình ảnh thành phố
            currentCityImage = Instantiate(cityImagePrefabs[index], cityImageParentContainer);
            currentCityImage.transform.localPosition = Vector3.zero;
            currentCityImage.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy prefab hình ảnh thành phố phù hợp cho levelIndex: " + levelIndex);
        }
    }

    IEnumerator MoveImages()
    {
        yield return new WaitForSeconds(0.5f);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            imageLeft.localPosition = Vector3.Lerp(leftImageStartPos, leftImageEndPos, elapsedTime / moveDuration);
            imageRightContainer.transform.localPosition = Vector3.Lerp(rightImageStartPos, rightImageEndPos, elapsedTime / moveDuration);
            yield return null;
        }

        imageLeft.localPosition = leftImageEndPos;
        imageRightContainer.transform.localPosition = rightImageEndPos;
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        panelCanvasGroup.alpha = 0f;
        panel.SetActive(false);

        if (levelIndex == 1 || levelIndex == 241)
        {
            GameManager.Instance.canShot = false;
        }
        else
        {
            GameManager.Instance.canShot = true;
        }
        Debug.Log("Turn on gun");
        ResetState();
    }

    private void ResetState()
    {
        if (currentRightImage != null)
        {
            Destroy(currentRightImage);
            currentRightImage = null;
        }

        if (currentCityImage != null)
        {
            Destroy(currentCityImage);
            currentCityImage = null;
        }

        imageLeft.localPosition = leftImageStartPos;
        imageRightContainer.transform.localPosition = rightImageStartPos;

        panelCanvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
}
