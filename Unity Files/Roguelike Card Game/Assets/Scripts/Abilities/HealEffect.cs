using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Effect
{
    public int amount;
    public bool affectAll;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
    {
        if (placePoint.activeCard == null)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_EMPTY);
            return false;
        }

        placePoint.activeCard.HealCard(amount);
        return true;
    }
}
