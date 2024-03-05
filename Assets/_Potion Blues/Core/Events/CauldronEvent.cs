using PotionBlues.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class CauldronEvent : IEvent
    {
        public CauldronEventType Type;
        public List<ShopAttributeValue> Attributes;
        // the attributes relevant to cauldrons are:
        //  - brewing speed
        //  - brewing output
        //  - cauldron capacity
        public IngredientDefinition Ingredient;
        public PotionDefinition Potion;
        public bool Accepted;

        public CauldronEvent(CauldronEventType t, List<ShopAttributeValue> attr)
        {
            Type = t;
            Attributes = attr;
        }
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
        // when a brew failed and the ingredients have to be scrapped
        BrewFailed,
        // when potions are removed from the cauldron
        PotionRemove,
    }
}