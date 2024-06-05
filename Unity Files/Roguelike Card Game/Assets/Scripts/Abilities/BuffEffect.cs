using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffEffect", menuName = "Effects/Buff", order = 1)]
public class BuffEffect : Effect
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

        if (playedByPlayer && !placePoint.isPlayerPoint)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_ENEMY);
            return false;
        }

        if (affectAll)
        {
            foreach (CardPlacePoint point in CardPointsController.instance.playerCardPoints)
            {
                if (point.activeCard != null)
                {
                    point.activeCard.BuffCard(amountAttack, amountDefense);
                }
            }
        }
        else
        {
            Debug.Log("Test2");
            placePoint.activeCard.BuffCard(amountAttack, amountDefense);
        }

        return true;
    }
}
