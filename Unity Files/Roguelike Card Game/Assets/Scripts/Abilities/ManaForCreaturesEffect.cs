using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewManaEffect", menuName = "Effects/ManaForCreatures", order = 1)]
public class ManaForCreaturesEffect : Effect
{
    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (playedByPlayer)
        {
            BattleController.instance.AddPlayerMana(CardPointsController.instance.PlayerCreatureCount());
        }
        else
        {
            BattleController.instance.AddEnemyMana(CardPointsController.instance.EnemyCreatureCount());
        }

        return true;
    }
}
