using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageEffect", menuName = "Effects/Damage", order = 1)]
public class DamageEffect : Effect
{
    public int amount;
    public bool affectAll;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (affectAll)
        {
            if (CardPointsController.instance.EnemyCreatureCount() <= 0)
            {
                BattleUIController.instance.ShowWarning(WARNING_CREATURES_ENEMY);
                return false;
            }
            else
            {
                foreach (CardPlacePoint point in CardPointsController.instance.enemyCardPoints)
                {
                    if (point.activeCard != null)
                    {
                        point.activeCard.DamageCard(amount);
                    }
                }
            }
        }
        else if (placePoint.activeCard == null)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_EMPTY);
            return false;
        }
        else if (playedByPlayer && placePoint.isPlayerPoint)
        {
            BattleUIController.instance.ShowWarning(WARNING_LAND_PLAYER);
            return false;
        }
        else
        {
            placePoint.activeCard.DamageCard(amount);
        }

        return true;
    }
}
