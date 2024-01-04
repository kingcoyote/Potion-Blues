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
                _instance = Transform.FindObjectOfType<PotionBlues>();
            }

            if (_instance == null)
            {
                _instance = CreateInstance();
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
            pb.EventBus = new GenericEventBus<IEvent, IEventNode>();
            pb.RNG = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            pb.PotionTypes = Resources.LoadAll<PotionDefinition>("Potions")
                .ToDictionary(potion => potion.name);
            pb.ShopObjectCategories = Resources.LoadAll<ShopObjectCategoryDefinition>("Object Categories")
                .ToDictionary(cat => cat.name);
            pb.GameData = new GameData();

            return pb;
        }
    }
}
