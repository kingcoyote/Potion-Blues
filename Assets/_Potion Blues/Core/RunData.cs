using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotionBlues
{
    [Serializable]
    public class RunData
    {
        [TableList]
        public List<RunUpgradeCard> Upgrades = new();
        [TableList]
        public List<RunUpgradeCard> MerchantCards = new();
        public float Gold;
        public float Reputation;
        public int Day;
        public int RunDuration;
        public List<CustomerTransaction> CustomerTransactions = new();

        [NonSerialized]
        private List<PotionDefinition> PotionCache;

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

        public PotionDefinition GetRandomPotion()
        {
            if (PotionCache == null)
            {
                BuildPotionCache();
            }

            if (PotionCache.Count == 0)
            {
                return null;
            } else if (PotionCache.Count == 1)
            {
                return PotionCache[0];
            }

            var randomIndex = PotionBlues.I().RNG.NextInt(PotionCache.Count);

            Debug.Log($"Next random potion is {randomIndex}");

            return PotionCache[randomIndex];
        }

        private void BuildPotionCache()
        {
            var pb = PotionBlues.I();

            var primaryIngredients = GetShopUpgrades(pb.ShopObjectCategories["Ingredient"])
                .Where(card => card.IsSelected)
                .Select(card => ((ShopObjectUpgradeCardDefinition)card.Card).ShopObject)
                .Select(card => (IngredientDefinition)card)
                .Where(ingredient => ingredient.Type == IngredientDefinition.IngredientType.Primary)
                .ToList();

            var potionTypes = PotionBlues.I().PotionTypes
                .Select(potion => potion.Value)
                .Where(potion => primaryIngredients.Intersect(potion.Ingredients).SequenceEqual(potion.Ingredients))
                .ToList();

            PotionCache = potionTypes;
        }

        public void InvalidatePotionCache()
        {
            PotionCache = null;
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

    [Serializable]
    public class CustomerTransaction
    {
        public PotionDefinition Potion;
        public float Gold;
        public float Reputation;
        public int Day;

        public CustomerTransaction(PotionDefinition potion, float gold, float reputation, int day)
        {
            Potion = potion;
            Gold = gold;
            Reputation = reputation;
            Day = day;
        }
    }
}