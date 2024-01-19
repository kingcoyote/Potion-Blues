using PotionBlues.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace PotionBlues
{
    [Serializable]
    public class RunData
    {
        public List<UpgradeCardDefinition> Upgrades = new();
        public int Gold;
        public int Reputation;
        public int Day;
        public int RunDuration;

        // difficulty
        // events
        // day history

        public List<ShopObjectUpgradeCardDefinition> GetShopUpgrades(ShopObjectCategoryDefinition category)
        {
            return GetCardsOfType<ShopObjectUpgradeCardDefinition>()
                .Where(card => card.ShopObject.Category == category)
                .ToList();
        }

        public List<ShopObjectDefinition> GetShopObjects(ShopObjectCategoryDefinition category)
        {
            return GetShopUpgrades(category)
                .Select(card => card.ShopObject)
                .ToList();
        }

        public List<ShopAttributeUpgradeCardDefintion> GetShopAttributeUpgrades()
        {
            return GetCardsOfType<ShopAttributeUpgradeCardDefintion>()
                .ToList();
        }

        private IEnumerable<TValue> GetCardsOfType<TValue>() where TValue : UpgradeCardDefinition
        {
            return Upgrades
                .Where(card => card.GetType() == typeof(TValue))
                .Select(card => (TValue)card);
        }
    }
}