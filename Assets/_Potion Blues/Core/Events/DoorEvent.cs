using PotionBlues.Definitions;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Events
{
    public class DoorEvent : IEvent
    {
        public List<ShopAttributeValue> Attributes;
        public DoorEventType Type;
        public PotionDefinition Potion;
    }

    public enum DoorEventType
    {
        Spawn,
        Despawn,

        // when a customer is spawned, this event is fired, and when the door receives it, the spawn rate is adjusted
        // but what about managing what type of potion the customer wants? is that fed in by the event listeners, too?
        // door is source and target
        CustomerArrive,

        // when a customer returns to the door, the event is fired, and the payout is recorded
        CustomerLeave
    }
}