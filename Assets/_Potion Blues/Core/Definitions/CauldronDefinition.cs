using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    public class CauldronDefinition : ShopObjectDefinition
    {
        public float BrewTime => _brewTime;
        public float BrewingOutput => _brewingOutput;

        [BoxGroup("Cauldron"), SerializeField] private float _brewTime;
        [BoxGroup("Cauldron"), SerializeField] private float _brewingOutput;
    }
}