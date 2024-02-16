using PotionBlues.Definitions;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class IngredientEvent : IEvent
    {
        public List<ShopAttributeValue> Attributes;
        public IngredientEventType Type;
        public IngredientDefinition Definition;

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
        Select,
        Use,
        Deselect,
        Salvage,
        Restock
    }
}