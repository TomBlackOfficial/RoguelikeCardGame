using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsEffect : Effect
{
    public int amount;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
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
