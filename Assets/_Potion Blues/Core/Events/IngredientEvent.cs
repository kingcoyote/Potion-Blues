using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class IngredientEvent : IEvent
    {
        public List<ShopAttributeValue> Attributes;
        public IngredientEventType Type;

        public IngredientEvent(IngredientEventType type, List<ShopAttributeValue> attributes)
        {
            Type = type;
            Attributes = attributes;
        }
    }

    public enum IngredientEventType
    {
        Spawn,
        Despawn,
        // when the player picks an ingredient (for ingredient cost)
        IngredientSelect
    }
}