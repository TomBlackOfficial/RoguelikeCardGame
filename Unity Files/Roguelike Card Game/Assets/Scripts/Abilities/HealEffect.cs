using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealEffect", menuName = "Effects/Heal", order = 1)]
public class HealEffect : Effect
{
    public int amount;
    public bool affectAll;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (affectAll)
        {
            if (CardPointsController.instance.PlayerCreatureCount() <= 0)
            {
                BattleUIController.instance.ShowWarning(WARNING_CREATURES_PLAYER);
                return false;
            }
            else
            {
                foreach (CardPlacePoint point in CardPointsController.instance.playerCardPoints)
                {
                    if (point.activeCard != null)
                    {
                        point.activeCard.HealCard(amount);
                    }
                }
            }
        }
        else if (placePoint.activeCard == null)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_EMPTY);
            return false;
        }
        else if (playedByPlayer && !placePoint.isPlayerPoint)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_ENEMY);
            return false;
        }
        else
        {
            placePoint.activeCard.HealCard(amount);
        }

        return true;
    }
}
