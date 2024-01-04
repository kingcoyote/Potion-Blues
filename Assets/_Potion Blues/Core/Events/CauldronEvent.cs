using PotionBlues.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class CauldronEvent : ShopObjectEvent
    {
        public CauldronEventType Type;
        // the attributes relevant to cauldrons are:
        //  - brewing speed
        //  - brewing output
        //  - cauldron capacity
        public IngredientDefinition Ingredient;
        public PotionDefinition Potion;
    }

    public enum CauldronEventType
    {
        // all events are raised by the cauldron, aimed at itself, allowing other scripts to intercept and alter stats
        // on creating the cauldron (cauldron capacity modified)
        Spawn,
        // on destroying the cauldron
        Despawn,
        // whenever an ingredient is added to the cauldron
        IngredientAdd,
        // when all ingredients are added and the brewing has begun (brewing speed modified)
        BrewStart,
        // when brewing is complete and potions are reading to be removed (brewing output modified)
        BrewFinish,
        // when potions are removed from the cauldron
        PotionRemove,
    }
}