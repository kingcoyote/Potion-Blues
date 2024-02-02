using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace PotionBlues
{
    [Serializable]
    public class RunData
    {
        [TableList]
        public List<RunUpgradeCard> Upgrades = new();
        [TableList]
        public List<RunUpgradeCard> MerchantCards = new();
        public int Gold;
        public int Reputation;
        public int Day;
        public int RunDuration;

        // difficulty
        // events
        // day history

        public List<RunUpgradeCard> GetShopObjects(ShopObjectCategoryDefinition category)
        {
            return GetShopUpgrades(category)
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

    [Serializable]
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