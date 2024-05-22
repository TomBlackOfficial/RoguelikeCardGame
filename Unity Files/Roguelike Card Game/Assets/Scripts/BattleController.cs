using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public enum TurnOrder
    {
        playerActive,
        playerCardAttacks,
        enemyActive,
        enemyCardAttacks
    }
    public TurnOrder currentPhase;

    [Header("Health Settings")]
    [SerializeField] private int maxPlayerHealth = 20;
    [SerializeField] private int maxEnemyHealth = 20;

    [Header("Mana Settings")]
    [SerializeField] private int startingMana = 5;
    [SerializeField] private int maxMana = 12;

    public int playerMana { private set; get; }
    public int enemyMana { private set; get; }

    private int currentPlayerMaxMana;
    private int currentEnemyMaxMana;

    [Header("Card Settings")]
    public int startingCards = 4;
    public int cardsPerTurn = 1;

    [Header("Assign")]
    public Transform discardPoint;

    public int playerHealth { private set; get; }
    public int enemyHealth { private set; get; }

    public bool battleEnded { private set; get; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerHealth = maxPlayerHealth;
        enemyHealth = maxEnemyHealth;
        BattleUIController.instance.UpdatePlayerHealthUI(PlayerHealthAmount());
        BattleUIController.instance.UpdateEnemyHealthUI(EnemyHealthAmount());

        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
        FillPlayerMana();
        FillEnemyMana();

        DeckController.instance.DrawMultipleCards(startingCards);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amount)
    {
        playerMana -= amount;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        BattleUIController.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amount)
    {
        enemyMana -= amount;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }

        BattleUIController.instance.SetEnemyManaText(enemyMana);
    }

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        BattleUIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        BattleUIController.instance.SetEnemyManaText(enemyMana);
    }


    public void AdvanceTurn()
    {
        if (battleEnded)
            return;

        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch(currentPhase)
        {
            case TurnOrder.playerActive:

                BattleUIController.instance.UpdatePlayerActionUI(true);

                if (currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();

                DeckController.instance.DrawMultipleCards(cardsPerTurn);

                break;

            case TurnOrder.playerCardAttacks:

                CardPointsController.instance.PlayerAttack();

                break;

            case TurnOrder.enemyActive:

                if (currentEnemyMaxMana < maxMana)
                {
                    currentEnemyMaxMana++;
                }

                FillEnemyMana();

                EnemyController.instance.StartAction();

                break;

            case TurnOrder.enemyCardAttacks:

                CardPointsController.instance.EnemyAttack();

                break;


        }
    }

    public void EndPlayerTurn()
    {
        BattleUIController.instance.UpdatePlayerActionUI(false);

        AdvanceTurn();
    }

    public void DamagePlayer(int amount)
    {
        if (playerHealth > 0 && !battleEnded)
        {
            playerHealth -= amount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;

                // End Battle
                EndBattle(false);
            }

            BattleUIController.instance.UpdatePlayerHealthUI(PlayerHealthAmount());
        }
    }

    public void DamageEnemy(int amount)
    {
        if (enemyHealth > 0 && !battleEnded)
        {
            enemyHealth -= amount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                // End Battle
                EndBattle(true);
            }

            BattleUIController.instance.UpdateEnemyHealthUI(EnemyHealthAmount());
        }
    }

    private void EndBattle(bool isWin)
    {
        battleEnded = true;

        StartCoroutine(ShowResultCoroutine(isWin));
    }

    IEnumerator ShowResultCoroutine(bool isWin)
    {
        yield return new WaitForSeconds(0.5f);

        BattleUIController.instance.ShowBattleEndScreen(isWin);
    }

    public bool CanPerformActions()
    {
        if (currentPhase == TurnOrder.playerActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float PlayerHealthAmount()
    {
        return (float)playerHealth / (float)maxPlayerHealth;
    }

    public float EnemyHealthAmount()
    {
        return (float)enemyHealth / (float)maxEnemyHealth;
    }
}
