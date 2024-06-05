using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaForCreaturesEffect : Effect
{
    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
    {
        if (playedByPlayer)
        {
            BattleController.instance.AddPlayerMana(CardPointsController.instance.PlayerCreatureCount());
        }
        else
        {
            BattleController.instance.AddEnemyMana(EnemyController.instance.cardsInHand.Count);
        }

        return true;
    }
}
