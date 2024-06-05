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
    [SerializeField] private TMP_Text playerLivesText;
    [SerializeField] private TMP_Text enemyManaText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Popup popupWarning;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject battleWinScreen;
    [SerializeField] private GameObject battleLoseScreen;

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

    public void SetEnemyManaText(int manaAmount)
    {
        enemyManaText.text = manaAmount.ToString();
    }

    public void UpdatePlayerHealthUI(float healthAmount)
    {
        playerHealthSlider.value = healthAmount;
    }

    public void UpdateEnemyHealthUI(float healthAmount)
    {
        enemyHealthSlider.value = healthAmount;
    }

    public void ShowBattleEndScreen(bool isWin)
    {
        if (isWin)
        {
            battleWinScreen.SetActive(true);
        }
        else
        {
            battleLoseScreen.SetActive(true);
        }
    }

    public void ShowWarning(string warningText)
    {
        currentPopup = popupWarning;
        popupWarning.ShowPopup(warningText);
        popupCounter = popupTime;
    }

    public void UpdatePlayerLivesUI(int lives)
    {
        if (playerLivesText == null)
            return;

        playerLivesText.text = "Lives: " + lives.ToString();
    }
}
