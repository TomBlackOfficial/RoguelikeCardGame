using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    [SerializeField] private int startingMana = 4, maxMana = 12;
    public int playerMana;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerMana = startingMana;
        BattleUIController.instance.SetPlayerManaText(playerMana);
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
}
