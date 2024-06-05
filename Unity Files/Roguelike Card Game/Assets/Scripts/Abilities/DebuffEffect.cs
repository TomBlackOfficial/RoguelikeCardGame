using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDebuffEffect", menuName = "Effects/Debuff", order = 1)]
public class DebuffEffect : Effect
{
    public int amountAttack;
    public int amountDefense;
    public bool affectAll;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (placePoint.activeCard == null)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_EMPTY);
            return false;
        }

        if (playedByPlayer && placePoint.isPlayerPoint) 
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_PLAYER);
            return false;
        }

        if (affectAll)
        {
            foreach (CardPlacePoint point in CardPointsController.instance.playerCardPoints)
            {
                if (point.activeCard != null)
                {
                    point.activeCard.DebuffCard(amountAttack, amountDefense);
                }
            }
        }
        else
        {
            placePoint.activeCard.DebuffCard(amountAttack, amountDefense);
        }

        return true;
    }
}
