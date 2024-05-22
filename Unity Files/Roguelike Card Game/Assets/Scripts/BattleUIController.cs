using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    public static BattleUIController instance;

    [Header("Settings")]
    [SerializeField] private float popupTime = 1f;

    [Header("Assign")]
    [SerializeField] private TMP_Text playerManaText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Popup popupWarning;
    [SerializeField] private Button endTurnButton;

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

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
    }

    public void UpdatePlayerActionUI(bool isActive)
    {
        endTurnButton.interactable = isActive;
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = manaAmount.ToString();
    }
    
    public void UpdatePlayerHealthUI(float healthAmount)
    {
        playerHealthSlider.value = healthAmount;
    }

    public void UpdateEnemyHealthUI(float healthAmount)
    {
        enemyHealthSlider.value = healthAmount;
    }

    public void ShowWarning(string warningText)
    {
        currentPopup = popupWarning;
        popupWarning.ShowPopup(warningText);
        popupCounter = popupTime;
    }
}
