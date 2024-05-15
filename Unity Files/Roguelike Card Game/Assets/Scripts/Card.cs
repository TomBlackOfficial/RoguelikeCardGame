using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int health { private set; get; }
    public int attack { private set; get; }
    public int manaCost { private set; get; }

    [SerializeField] private TMP_Text nameText, descriptionText, healthText, attackText, costText;

    private void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        health = cardSO.health;
        attack = cardSO.attack;
        manaCost = cardSO.manaCost;

        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        costText.text = manaCost.ToString();

        nameText.text = cardSO.cardName;
        descriptionText.text = cardSO.description;
    }
}
