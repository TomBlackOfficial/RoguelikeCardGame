using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIController : MonoBehaviour
{
    public static BattleUIController instance;

    [SerializeField] private TMP_Text playerManaText;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = manaAmount.ToString();
    }
}
