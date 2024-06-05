using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceManaCostEffect : Effect
{
    public int amount = 1;

    public override bool OnUse(bool playedByPlayer, CardPlacePoint placePoint)
    {
        if (!playedByPlayer)
        {
            return false;
        }

        HandController.instance.ReduceHandManaCost(amount);
        return true;
    }
}
