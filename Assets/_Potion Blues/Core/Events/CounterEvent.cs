using PotionBlues.Definitions;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class CounterEvent : ShopObjectEvent
    {
        public CounterEventType Type;
        public PotionDefinition Potion;
    }

    public enum CounterEventType
    {
        // allows changing of base stats, like counter slots
        Spawn,
        Despawn,
        // when a new potion is added to the counter, fire this event (for... potion dupication? decay rate setting?)
        PotionAdd,
        // when a potion has gone bad, fire this event (for recovery rates?)
        PotionSpoil,
        // when a potion has been purchased, fire this event (for potion value and tipping)
        PotionPurchase,
    }
}