using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : Effect
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

        placePoint.activeCard.DamageCard(amount);
        return true;
    }
}
