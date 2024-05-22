using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private float targetAlpha;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        targetAlpha = 0f;
    }

    private void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, 10f * Time.deltaTime);
        }
    }

    public void ShowPopup(string textToShow)
    {
        popupText.text = textToShow;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        targetAlpha = 1;
    }

    public void HidePopup()
    {
        targetAlpha = 0f;
    }
}
