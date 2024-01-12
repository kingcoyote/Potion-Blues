using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Cauldron")]
    public class CauldronDefinition : ShopObjectDefinition
    {
        public float BrewingTime => _brewingTime;
        public float BrewingOutput => _brewingOutput;
        public float CleaningInterval => _cleaningInterval;
        public float FailureRate => _failureRate;

        [BoxGroup("Cauldron"), SerializeField] private float _brewingTime;
        [BoxGroup("Cauldron"), SerializeField] private float _brewingOutput;
        [BoxGroup("Cauldron"), SerializeField] private float _cleaningInterval;
        [BoxGroup("Cauldron"), SerializeField] private float _failureRate;
    }
}