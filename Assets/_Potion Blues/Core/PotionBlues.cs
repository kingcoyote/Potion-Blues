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
        public GameData Data;

        public List<PotionDefinition> PotionTypes;

        public GenericEventBus<IEvent, IEventNode> EventBus;
        public Unity.Mathematics.Random RNG;

        public static PotionBlues _instance;
        public static PotionBlues I()
        {
            if (_instance == null)
            {
                _instance = CreateInstance();
            }

            return _instance;
        }

        private static PotionBlues CreateInstance()
        {
            var go = new GameObject();
            DontDestroyOnLoad(go);

            var pb = go.AddComponent<PotionBlues>();
            pb.EventBus = new GenericEventBus<IEvent, IEventNode>();
            pb.RNG = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            pb.PotionTypes = Resources.LoadAll<PotionDefinition>("Potion Types").ToList();

            return pb;
        }
    }
}
