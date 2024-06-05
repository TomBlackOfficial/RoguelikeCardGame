using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaForCardsEffect : Effect
{
    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
    {
        if (playedByPlayer)
        {
            BattleController.instance.AddPlayerMana(HandController.instance.heldCards.Count);
        }
        else
        {
            BattleController.instance.AddEnemyMana(CardPointsController.instance.EnemyCreatureCount());
        }

        return true;
    }
}
