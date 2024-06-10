using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewManaEffect", menuName = "Effects/ManaForCards", order = 1)]
public class ManaForCardsEffect : Effect
{
    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (playedByPlayer)
        {
            BattleController.instance.AddPlayerMana(HandController.instance.heldCards.Count - 1);
        }
        else
        {
            BattleController.instance.AddEnemyMana(EnemyController.instance.cardsInHand.Count - 1);
        }

        return true;
    }
}
