using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform pausePanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button ingameMenuButton;
    [SerializeField]
    private Transform objectHolderContainer;
    [SerializeField]
    private Color disabledColor;
    [SerializeField]
    private Text levelText;

    private void Start()
    {
        levelText.text = "Level " + (LevelManager.instance.currentLevelIndex + 1).ToString();
    }

    public void ChangeObjQuantity(int index, int quantity)
    {
        Transform holder = objectHolderContainer.GetChild(index);
        holder.GetChild(0).GetComponent<Text>().text = quantity.ToString();
        if(quantity == 0)
        {
            holder.GetComponent<Image>().color = disabledColor;
            holder.GetComponent<CanvasGroup>().interactable = false;
            holder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void ShowPausePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), pausePanel.GetComponent<RectTransform>());
        ingameMenuButton.interactable = false;
        Time.timeScale = 0; 
    }

    public void HidePausePanel()
    {
        StartCoroutine(FadeOut(overlayPanel.GetComponent<CanvasGroup>(), pausePanel.GetComponent<RectTransform>()));
        ingameMenuButton.interactable = true;
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        ingameMenuButton.interactable = false;
        StartCoroutine(SetAchive(winPanel));
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        ingameMenuButton.interactable = false;
        StartCoroutine(SetAchive(losePanel));
    }

    private IEnumerator SetAchive(Transform achiveParent)
    {
        Transform achiveContainer = achiveParent.GetChild(1);
        for(int i = 0; i < achiveContainer.childCount; i++)
        {
            yield return new WaitForSecondsRealtime(.3f);
            if(i < GameManager.instance.achivement)
            {
                Transform star = achiveContainer.GetChild(i);
                SetStar(star);
            }
        }
    }

    private void SetStar(Transform star)
    {
        star.localScale = new Vector3(0, 0, 0);
        star.gameObject.SetActive(true);
        star.DOScale(new Vector3(1, 1, 1), .3f).SetEase(Ease.OutBack).SetUpdate(true);
        Transform starDisabler = star.GetChild(0);
        starDisabler.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 400), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);

        pausePanel.gameObject.SetActive(false);
        overlayPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
