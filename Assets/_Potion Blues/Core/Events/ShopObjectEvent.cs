using System.Collections.Generic;

namespace PotionBlues.Events
{
    public class ShopObjectEvent : IEvent
    {
        public List<ShopAttributeValue> Attributes;
    }
}