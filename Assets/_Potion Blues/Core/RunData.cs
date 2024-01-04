using PotionBlues.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotionBlues
{
    [Serializable]
    public class RunData
    {
        public List<UpgradeCardDefinition> Upgrades = new();
        public int Gold;
        public int Reputation;
        public int Day;

        // difficulty
        // events
        // day history

        public List<ShopObjectDefinition> GetShopObjects(ShopObjectCategoryDefinition category)
        {
            return Upgrades
                .Where(card => card.GetType() == typeof(ShopObjectUpgradeCardDefintion))
                .Select(card => (ShopObjectUpgradeCardDefintion)card)
                .Where(card => card.ShopObject.Category == category)
                .Select(card => card.ShopObject)
                .ToList();
        }
    }
}