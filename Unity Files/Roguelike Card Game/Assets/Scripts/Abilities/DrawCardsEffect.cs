using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffEffect", menuName = "Effects/DrawCards", order = 1)]
public class DrawCardsEffect : Effect
{
    public int amount;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (playedByPlayer)
        {
            DeckController.instance.DrawMultipleCards(amount);
        }
        else
        {
            EnemyController.instance.DrawMultipleCards(amount);
        }

        return true;
    }
}
