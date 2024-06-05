using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffEffect : Effect
{
    public int amountAttack;
    public int amountHealth;
    public bool affectAll;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
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

        placePoint.activeCard.DebuffCard(amountAttack, amountHealth);
        return true;
    }
}
