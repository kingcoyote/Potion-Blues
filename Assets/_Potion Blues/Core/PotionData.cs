using PotionBlues.Definitions;
using System.Collections.Generic;

namespace PotionBlues
{
    public class PotionData
    {
        public PotionDefinition Potion;
        public List<ShopAttributeValue> Attributes;

        public PotionData(PotionDefinition potion, List<ShopAttributeValue> attributes)
        {
            Potion = potion;
            Attributes = attributes;
        }
    }
}