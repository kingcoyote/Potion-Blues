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
        public List<RunUpgradeCard> Upgrades = new();
        public List<RunUpgradeCard> MerchantCards = new();
        public int Gold;
        public int Reputation;
        public int Day;
        public int RunDuration;

        // difficulty
        // events
        // day history

        public List<ShopObjectDefinition> GetShopObjects(ShopObjectCategoryDefinition category)
        {
            return GetShopUpgrades(category)
                .Select(card => ((ShopObjectUpgradeCardDefinition)card.Card).ShopObject)
                .ToList();
        }

        public List<RunUpgradeCard> GetShopAttributeUpgrades()
        {
            return GetCardsOfType<ShopAttributeUpgradeCardDefintion>()
                .ToList();
        }

        private List<RunUpgradeCard> GetShopUpgrades(ShopObjectCategoryDefinition category)
        {
            return GetCardsOfType<ShopObjectUpgradeCardDefinition>()
                .Where(card => ((ShopObjectUpgradeCardDefinition)card.Card).ShopObject.Category == category)
                .ToList();
        }

        private IEnumerable<RunUpgradeCard> GetCardsOfType<TValue>() where TValue : UpgradeCardDefinition
        {
            return Upgrades
                .Where(card => card.Card.GetType() == typeof(TValue));
        }
    }

    public class RunUpgradeCard
    {
        public UpgradeCardDefinition Card;
        public bool IsSelected;

        public RunUpgradeCard(UpgradeCardDefinition card)
        {
            Card = card;
        }
    }
}