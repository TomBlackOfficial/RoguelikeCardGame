using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIController : MonoBehaviour
{
    public static BattleUIController instance;

    [Header("Settings")]
    [SerializeField] private float popupTime = 1f;

    [Header("Assign")]
    [SerializeField] private TMP_Text playerManaText;
    [SerializeField] private Popup popupWarning;

    private Popup currentPopup;
    private float popupCounter;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (popupCounter > 0 && currentPopup != null)
        {
            popupCounter -= Time.deltaTime;

            if (popupCounter <= 0 )
            {
                currentPopup.HidePopup();
            }
        }
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = manaAmount.ToString();
    }
    
    public void ShowWarning(string warningText)
    {
        currentPopup = popupWarning;
        popupWarning.ShowPopup(warningText);
        popupCounter = popupTime;
    }
}
