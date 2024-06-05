using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    protected static string WARNING_LAND_EMPTY = "The selected land tile is empty.";
    protected static string WARNING_LAND_PLAYER = "You can only use this on enemy creatures.";
    protected static string WARNING_LAND_ENEMY = "You can only use this on your creatures.";

    public abstract bool OnUse(bool playedByPlayer, CardPlacePoint placePoint);
}
