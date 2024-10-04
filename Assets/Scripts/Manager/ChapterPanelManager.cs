using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanelManager : MonoBehaviour
{
    public CanvasGroup panelCanvasGroup;
    public GameObject panel;
    public float fadeDuration = 1f;

    public RectTransform imageLeft;   
    public RectTransform imageRight;  
    public float moveDuration = 1f;   
    private Vector3 leftImageStartPos; 
    private Vector3 rightImageStartPos;
    private Vector3 leftImageEndPos;   
    private Vector3 rightImageEndPos; 

    void Start()
    {
        leftImageEndPos = imageLeft.localPosition;
        rightImageEndPos = imageRight.localPosition;

        leftImageStartPos = leftImageEndPos + new Vector3(-500f, 0f, 0f);
        rightImageStartPos = rightImageEndPos + new Vector3(500f, 0f, 0f);

        imageLeft.localPosition = leftImageStartPos;
        imageRight.localPosition = rightImageStartPos;

        StartCoroutine(MoveImages());
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
    }

    IEnumerator MoveImages()
    {
        yield return new WaitForSeconds(0.5f);
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            imageLeft.localPosition = Vector3.Lerp(leftImageStartPos, leftImageEndPos, elapsedTime / moveDuration);
            imageRight.localPosition = Vector3.Lerp(rightImageStartPos, rightImageEndPos, elapsedTime / moveDuration);

            yield return null;
        }

        imageLeft.localPosition = leftImageEndPos;
        imageRight.localPosition = rightImageEndPos;
        StartCoroutine(FadeOutPanel());

    }
}
