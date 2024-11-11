using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanelManager : MonoBehaviour
{
    public CanvasGroup panelCanvasGroup;
    public GameObject panel;
    public float fadeDuration = 1f;

    public RectTransform imageLeft;
    public GameObject imageRightContainer;
    public List<GameObject> rightImageParents;
    public List<GameObject> cityImages;
    public float moveDuration = 1f;
    private Vector3 leftImageStartPos;
    private Vector3 rightImageStartPos;
    private Vector3 leftImageEndPos;
    private Vector3 rightImageEndPos;
    public TextMeshProUGUI chapterText;
    public TextMeshProUGUI mapText;
    private int levelIndex;


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
        if (levelIndex == 1)
        {
            chapterText.text = "CHAPTER 1";
            mapText.text = "BULLET CITY";
        }
        else if (levelIndex == 17)
        {
            chapterText.text = "CHAPTER 2";
            mapText.text = "SHOGUN'S CASTLE";
        }
        else if (levelIndex == 33)
        {
            chapterText.text = "CHAPTER 3";
            mapText.text = "GRAVEYARD";
        }
        else if (levelIndex == 49)
        {
            chapterText.text = "CHAPTER 4";
            mapText.text = "FAR WEST";
        }
        else if (levelIndex == 65)
        {
            chapterText.text = "CHAPTER 5";
            mapText.text = "FOREST";
        }
        else if (levelIndex == 81)
        {
            chapterText.text = "CHAPTER 6";
            mapText.text = "FORTRESS";
        }
        else if (levelIndex == 97)
        {
            chapterText.text = "CHAPTER 7";
            mapText.text = "PREHISTORY";
        }
        else if (levelIndex == 113)
        {
            chapterText.text = "CHAPTER 8";
            mapText.text = "UNKNOW PLANET";
        }
        if (levelIndex == 1 || levelIndex == 17 || levelIndex == 33 || levelIndex == 49 || levelIndex == 65 || levelIndex == 81 || levelIndex == 97 || levelIndex == 113)
        {
            //playerarm = GameObject.FindWithTag("Gun");
            //Debug.Log("Found gun with name: " + playerarm?.name + " and tag: " + playerarm?.tag);
            //StartCoroutine(TurnOffPlayerArm());
            GameManager.Instance.canShot = false;
            Debug.Log("turn off gun");
            panel.SetActive(true);
            panelCanvasGroup.alpha = 1f;
            SetActiveImage(levelIndex);
            StartCoroutine(MoveImages());
        }
        else
        {
            GameManager.Instance.canShot = true;
        }
    }
    /*private IEnumerator TurnOffPlayerArm()
    {
        yield return null; // wait 1 frame
        if (playerarm != null)
        {
            playerarm.SetActive(false);
            Debug.Log("turn off gun in coroutine");
        }
    }*/
    private void SetActiveImage(int levelIndex)
    {
        foreach (var imgParent in rightImageParents)
        {
            imgParent.SetActive(false);
        }
        foreach (var cityImage in cityImages)
        {
            cityImage.SetActive(false);
        }
        if (levelIndex == 1 && rightImageParents.Count > 0 && cityImages.Count > 0)
        {
            rightImageParents[0].SetActive(true);
            cityImages[0].SetActive(true);
        }
        else if (levelIndex == 17 && rightImageParents.Count > 1 && cityImages.Count > 1)
        {
            rightImageParents[1].SetActive(true);
            cityImages[1].SetActive(true);
        }
        else if (levelIndex == 33 && rightImageParents.Count > 2 && cityImages.Count > 2)
        {
            rightImageParents[2].SetActive(true); 
            cityImages[2].SetActive(true);
        }
        else if (levelIndex == 49 && rightImageParents.Count > 3 && cityImages.Count > 3)
        {
            rightImageParents[3].SetActive(true);
            cityImages[3].SetActive(true);
        }
        else if (levelIndex == 65 && rightImageParents.Count > 4 && cityImages.Count > 4)
        {
            rightImageParents[4].SetActive(true);
            cityImages[4].SetActive(true);
        }
        else if (levelIndex == 81 && rightImageParents.Count > 5 && cityImages.Count > 5)
        {
            rightImageParents[5].SetActive(true);
            cityImages[5].SetActive(true);
        }
        else if (levelIndex == 97 && rightImageParents.Count > 6 && cityImages.Count > 6)
        {
            rightImageParents[6].SetActive(true);
            cityImages[6].SetActive(true);
        }
        else if (levelIndex == 113 && rightImageParents.Count > 7 && cityImages.Count > 7)
        {
            rightImageParents[7].SetActive(true);
            cityImages[7].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy hình ảnh phù hợp cho levelIndex: " + levelIndex);
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
        if (PlayerPrefs.GetString("SelectedMode") == "Classic" && levelIndex == 1)
        {
            GameManager.Instance.canShot = false;
        }
        else
        {
            GameManager.Instance.canShot = true;
        }
        Debug.Log("turn on gun");
        ResetState();
    }

    private void ResetState()
    {
        imageLeft.localPosition = leftImageStartPos;
        imageRightContainer.transform.localPosition = rightImageStartPos;

        panelCanvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
}
