using System.Collections;
using System.Collections.Generic;
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

        if (levelIndex == 1 || levelIndex == 17 || levelIndex == 33)
        {
            panel.SetActive(true);
            panelCanvasGroup.alpha = 1f;
            SetActiveImage(levelIndex);
            StartCoroutine(MoveImages());
        }
    }

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
        else
        {
            Debug.LogWarning("Không tìm thấy hình ảnh phù hợp cho levelIndex: " + levelIndex);
        }
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

        ResetState();
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

    private void ResetState()
    {
        imageLeft.localPosition = leftImageStartPos;
        imageRightContainer.transform.localPosition = rightImageStartPos;

        panelCanvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
}
