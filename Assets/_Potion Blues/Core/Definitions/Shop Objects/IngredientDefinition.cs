using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Ingredient")]
    public class IngredientDefinition : ShopObjectDefinition
    {
        public Sprite Ingredient => _ingredient;
        public IngredientType Type { get
            {
                return Attributes.Count == 4 ? IngredientType.Primary : IngredientType.Secondary;
            } 
        }

        [BoxGroup("Ingredient"), SerializeField, PreviewField, Tooltip("Sprite used as the ingredient picked up from the dispenser")] 
        private Sprite _ingredient;

        public enum IngredientType
        {
            Primary,
            Secondary
        }
    }
}