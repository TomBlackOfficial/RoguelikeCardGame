using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public enum Type
    {
        Creature,
        Spell
    }
    
    public enum CreatureState
    {
        Spawn,
        Attack,
        TakingDamage,
        Death
    }


    public string cardName;

    [TextArea]
    public string description;

    [Space(10)]
    public int manaCost = 1;
    public int health = 1;
    public int attack = 1;

    [Space(10)]
    public Type cardType = Type.Creature;
    

    [Space(10)]
    public GameObject creatureModel;
    
    [Space(10)]
    public GameObject creatureOnCardModel;

    [Space(10)]
    public List<Effect> effects = new List<Effect>();
    
    [Space(10)]
    public CreatureState creatureState = CreatureState.Spawn;
    
    [Space(10)]
    public AudioClip SpawnClip;
    public AudioClip AttackClip;
    public AudioClip TakingDamageClip;
    public AudioClip DeathClip;
}
