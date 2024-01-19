using System;
using System.Collections.Generic;
using System.Linq;
using GenericEventBus;
using UnityEngine;
using PotionBlues.Definitions;

namespace PotionBlues
{
    public class PotionBlues : MonoBehaviour
    {
        [SerializeField]
        public GameData GameData;

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

            if (_instance.EventBus == null)
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
            pb.GameData = GameData.Load("test");

            return pb;
        }

        private void Initialize()
        {
            EventBus = new GenericEventBus<IEvent, IEventNode>();
            RNG = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            PotionTypes = Resources.LoadAll<PotionDefinition>("Potions")
                .ToDictionary(potion => potion.name);
            ShopObjectCategories = Resources.LoadAll<ShopObjectCategoryDefinition>("Object Categories")
                .ToDictionary(cat => cat.name);
        }

        public void StartNewRun()
        {
            GameData.ActiveRun = GameData.GenerateRunData();
            EventBus.Raise(new RunEvent(RunEventType.Created));
        }

        public List<UpgradeCardDefinition> GetMerchantCards(int count)
        {
            return GameData.Upgrades
                .Except(GameData.ActiveRun.Upgrades)
                .OrderBy(x => Guid.NewGuid())
                .Take(count).ToList();
        }
    }
}
