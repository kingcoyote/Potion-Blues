using System;
using System.Collections.Generic;
using System.Linq;
using GenericEventBus;
using UnityEngine;
using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;

namespace PotionBlues
{
    public class PotionBlues : MonoBehaviour
    {
        [SerializeField]
        public GameData GameData;

        public List<UpgradeCardDefinition> Upgrades = new();
        public Dictionary<string, PotionDefinition> PotionTypes = new();
        public Dictionary<string, ShopObjectCategoryDefinition> ShopObjectCategories = new();

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
                IndexReferenceResolver = new ScriptableObjectIndexReferenceResolver()
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
                IndexReferenceResolver = new ScriptableObjectIndexReferenceResolver()
            };
            GameData = SerializationUtility.DeserializeValue<GameData>(bytes, DataFormat.JSON, context);
        }

        private static string GetFilePath(string name)
        {
            return Path.Join(Application.persistentDataPath, name);
        }

        public List<UpgradeCardDefinition> GetMerchantCards(int count)
        {
            return GameData.Upgrades
                .Except(GameData.ActiveRun.Upgrades.Select(card => card.Card))
                .OrderBy(x => Guid.NewGuid())
                .Take(count).ToList();
        }
    }

    public class ScriptableObjectIndexReferenceResolver : IExternalIndexReferenceResolver
    {
        // Multiple string reference resolvers can be chained together.
        public IExternalStringReferenceResolver NextResolver { get; set; }

        public bool CanReference(object value, out int id)
        {
            if (value is UpgradeCardDefinition)
            {
                id = (value as UpgradeCardDefinition).GetInstanceID();
                return true;
            }

            id = -1;
            return false;
        }

        public bool TryResolveReference(int id, out object value)
        {
            value = Resources.InstanceIDToObject(id);
            return value != null;
        }
    }
}
