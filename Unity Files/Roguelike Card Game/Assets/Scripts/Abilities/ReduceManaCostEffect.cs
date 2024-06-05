using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewManaCostEffect", menuName = "Effects/ReduceManaCost", order = 1)]
public class ReduceManaCostEffect : Effect
{
    public int amount = 1;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed)
    {
        if (!playedByPlayer)
        {
            return false;
        }

        HandController.instance.ReduceHandManaCost(amount, cardPlayed);
        return true;
    }
}
