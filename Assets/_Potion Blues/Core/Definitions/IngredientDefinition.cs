using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Ingredient")]
    public class IngredientDefinition : ShopObjectDefinition
    {
        public Sprite Dispenser => _dispenser;
        public Sprite Ingredient => _ingredient;
        public bool Enhancer => _enhancer;
        public float Value => _value;

        [BoxGroup("Ingredient"), SerializeField, PreviewField, Tooltip("Sprite used as the dispenser for the player to pick up ingredients")]
        private Sprite _dispenser;
        [BoxGroup("Ingredient"), SerializeField, PreviewField, Tooltip("Sprite used as the ingredient picked up from the dispenser")] 
        private Sprite _ingredient;
        [BoxGroup("Ingredient"), SerializeField] private bool _enhancer;
        [BoxGroup("Ingredient"), SerializeField] private float _value;
    }
}