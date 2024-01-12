using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Counter")]
    public class CounterDefinition : ShopObjectDefinition
    {
        public int CounterSlots => _counterSlots;
        public float ShelfLife => _shelfLife;
        public float SlotCapacity => _slotCapacity;

        [BoxGroup("Counter"), SerializeField, Tooltip("Number of unique potions this counter can contain")] private int _counterSlots;
        [BoxGroup("Counter"), SerializeField, Tooltip("Quantity of potions each slot can hold")] private float _slotCapacity;
        [BoxGroup("Counter"), SerializeField, Tooltip("Number of seconds a potion can sit before rotting")] private float _shelfLife;
    }
}