using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Cauldron")]
    public class CauldronDefinition : ShopObjectDefinition
    {
        public Sprite Fill => _fill;
        public Sprite Dirty => _dirty;

        [SerializeField, BoxGroup("Cauldron"), Tooltip("Sprite used to show when the cauldron has liquid in it"), PreviewField] 
        private Sprite _fill;
        [SerializeField, BoxGroup("Cauldron"), Tooltip("Sprite used to show when the cauldron is dirty"), PreviewField] 
        private Sprite _dirty;
    }
}