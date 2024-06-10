using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    protected static string WARNING_LAND_EMPTY = "The selected land tile is empty.";
    protected static string WARNING_LAND_PLAYER = "You can only use this spell on enemy creatures.";
    protected static string WARNING_LAND_ENEMY = "You can only use this spell on your creatures.";
    protected static string WARNING_CREATURES_PLAYER = "You don't have any creatures on the board.";
    protected static string WARNING_CREATURES_ENEMY = "The enemy has no creatures on the board.";

    public abstract bool OnUse(bool playedByPlayer, CardPlacePoint placePoint, Card cardPlayed);
}
