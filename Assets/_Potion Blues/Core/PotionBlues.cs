using System;
using System.Collections.Generic;
using System.Linq;
using GenericEventBus;
using UnityEngine;
using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
using PotionBlues.Shop;

namespace PotionBlues
{
    public class PotionBlues : MonoBehaviour
    {
        [SerializeField]
        public GameData GameData;

        public List<UpgradeCardDefinition> Upgrades = new();
        public Dictionary<string, PotionDefinition> PotionTypes = new();
        public Dictionary<string, ShopObjectCategoryDefinition> ShopObjectCategories = new();
        public Dictionary<string, ShopAttributeDefinition> ShopAttributeDefinitions = new();

        public GenericEventBus<IEvent, IEventNode> EventBus;
        public Unity.Mathematics.Random RNG;

        public static PotionBlues _instance;
        public static PotionBlues I()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PotionBlues>();
            }

            if (_instance == null)
            {
                _instance = CreateInstance();
            }

            // depending on how the instance is created, it might be invalid and need to be re-initialized
            if (_instance.EventBus == null || _instance.Upgrades.Count() == 0)
            {
                _instance.Initialize();
            }

            return _instance;
        }

        private static PotionBlues CreateInstance()
        {
            var go = new GameObject();
            go.name = "Potion Blues";

            if (Application.isPlaying)
                DontDestroyOnLoad(go);

            var pb = go.AddComponent<PotionBlues>();

            pb.Initialize();

            return pb;
        }

        [Button]
        private void Initialize()
        {
            Debug.Log("Initializing PotionBlues");

            EventBus = new GenericEventBus<IEvent, IEventNode>();
            RNG = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            PotionTypes = Resources.LoadAll<PotionDefinition>("Potions")
                .ToDictionary(potion => potion.name);
            ShopObjectCategories = Resources.LoadAll<ShopObjectCategoryDefinition>("Object Categories")
                .ToDictionary(cat => cat.name);
            ShopAttributeDefinitions = Resources.LoadAll<ShopAttributeDefinition>("Shop Attributes")
                .ToDictionary(sad => sad.name);
            Upgrades = Resources.LoadAll<UpgradeCardDefinition>("Upgrades").ToList();

            LoadProfile("default");
        }

        public void StartNewRun()
        {
            GameData.ActiveRun = GameData.GenerateRunData();
        }

        [Button]
        public void Save()
        {
            string filename = GetFilePath(GameData.ProfileName);

            var context = new SerializationContext()
            {
                StringReferenceResolver = new UpgradeCardResolver()
            };
            var data = SerializationUtility.SerializeValue(GameData, DataFormat.JSON, context);

            using var fs = new FileStream(filename, FileMode.Create);

            Debug.Log($"Saved data to {filename}");

            fs.Write(data, 0, data.Length);
        }

        [Button]
        public void LoadProfile(string name)
        {
            string filename = GetFilePath(name);

            Debug.Log($"Loading GameData from {filename}");

            if (File.Exists(filename) == false)
            {
                GameData = new GameData("default");
                return;
            }

            var bytes = File.ReadAllBytes(filename);
            var context = new DeserializationContext()
            {
                StringReferenceResolver = new UpgradeCardResolver()
            };
            GameData = SerializationUtility.DeserializeValue<GameData>(bytes, DataFormat.JSON, context);
        }

        private static string GetFilePath(string name)
        {
            return Path.Join(Application.persistentDataPath, $"{name}.game");
        }

        public List<UpgradeCardDefinition> GetMerchantCards(int count)
        {
            return GameData.Upgrades
                .Except(GameData.ActiveRun.Upgrades.Select(card => card.Card))
                .OrderBy(x => Guid.NewGuid())
                .Take(count).ToList();
        }

        public PotionDefinition GetPotionType(List<IngredientDefinition> ingredients)
        {
            var primaryIngredients = ingredients.Where(i => i.Type == IngredientDefinition.IngredientType.Primary);

            var potionType = PotionTypes
                .Select(p => p.Value)
                .First(p => p.Ingredients.SequenceEqual(primaryIngredients));

            return potionType;
        }
    }

    public class UpgradeCardResolver : IExternalStringReferenceResolver
    {
        // Multiple string reference resolvers can be chained together.
        public IExternalStringReferenceResolver NextResolver { get; set; }

        public bool CanReference(object value, out string id)
        {
            if (value is UpgradeCardDefinition)
            {
                id = (value as UpgradeCardDefinition).Reference;
                return true;
            }

            id = "";
            return false;
        }

        public bool TryResolveReference(string id, out object value)
        {
            value = Resources.Load<UpgradeCardDefinition>($"Upgrades/{id}");
            return value != null;
        }
    }
}
