using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Cauldron")]
    public class CauldronDefinition : ShopObjectDefinition
    {
        public float BrewTime => _brewTime;
        public float BrewingOutput => _brewingOutput;

        [BoxGroup("Cauldron"), SerializeField] private float _brewTime;
        [BoxGroup("Cauldron"), SerializeField] private float _brewingOutput;
    }
}