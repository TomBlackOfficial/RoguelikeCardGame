using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int currentHealth, attackPower, manaCost;

    [SerializeField] private TMP_Text healthText, attackText, costText;
}
