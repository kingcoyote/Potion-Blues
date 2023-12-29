using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Counter")]
    public class CounterDefinition : ShopObjectDefinition
    {
        public int CounterSlots => _counterSlots;
        public float DecayRate => _decayRate;

        [BoxGroup("Counter"), SerializeField, Tooltip("Number of unique potions this counter can contain")] private int _counterSlots;
        [BoxGroup("Counter"), SerializeField, Tooltip("Amount of potion decay per second applied")] private float _decayRate;

    }
}