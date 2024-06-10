using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class DeckScriptableObject : ScriptableObject
{
    public List<CardScriptableObject> cards = new List<CardScriptableObject>();
    
     public void InitializeDeck(List<CardScriptableObject> newCards)
        {
            cards.Clear();
            cards.AddRange(newCards);
        }
}
