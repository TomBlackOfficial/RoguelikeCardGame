using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDrawCards", menuName = "Effects/CardsForCreatures", order = 1)]
public class CardsForCreatures : Effect
{
    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (playedByPlayer)
        {
            DeckController.instance.DrawMultipleCards(CardPointsController.instance.PlayerCreatureCount());
        }
        else
        {
            EnemyController.instance.DrawMultipleCards(CardPointsController.instance.EnemyCreatureCount());
        }

        return true;
    }
}
