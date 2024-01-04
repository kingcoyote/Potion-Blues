using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class IngredientEvent : ShopObjectEvent
    {
        public IngredientEventType Type;
    }

    public enum IngredientEventType
    {
        Spawn,
        Despawn,
        // when the player picks an ingredient (for ingredient cost)
        IngredientSelect
    }
}