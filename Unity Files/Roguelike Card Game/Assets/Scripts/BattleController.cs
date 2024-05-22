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
    private int currentPlayerMaxMana;

    [Header("Card Settings")]
    [SerializeField] private int startingCards = 4;
    [SerializeField] private int cardsPerTurn = 1;

    [Header("Assign")]
    public Transform discardPoint;

    public int playerHealth { private set; get; }
    public int enemyHealth { private set; get; }

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
        FillPlayerMana();
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

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        BattleUIController.instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn()
    {
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

                Debug.Log("Skipping enemy actions.");
                AdvanceTurn();

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
        if (playerHealth > 0)
        {
            playerHealth -= amount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;

                // End Battle
            }

            BattleUIController.instance.UpdatePlayerHealthUI(PlayerHealthAmount());
        }
    }

    public void DamageEnemy(int amount)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= amount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                // End Battle
            }

            BattleUIController.instance.UpdateEnemyHealthUI(EnemyHealthAmount());
        }
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
