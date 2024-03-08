using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Cauldron")]
    public class CauldronDefinition : ShopObjectDefinition
    {
        public Sprite Fill => _fill;

        [SerializeField, BoxGroup("Cauldron"), Tooltip("Sprite used to show when the cauldron has liquid in it")] private Sprite _fill;
    }
}