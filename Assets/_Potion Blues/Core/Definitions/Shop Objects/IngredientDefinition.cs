using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Ingredient")]
    public class IngredientDefinition : ShopObjectDefinition
    {
        public float IngredientCost => _cost;
        public float IngredientSalvage => _salvage;
        public float IngredientCooldown => _cooldown;
        public float IngredientQuantity => _quantity;
        public Sprite Ingredient => _ingredient;

        [BoxGroup("Ingredient"), SerializeField] public float _cost;
        [BoxGroup("Ingredient"), SerializeField] public float _salvage;
        [BoxGroup("Ingredient"), SerializeField] public float _cooldown;
        [BoxGroup("Ingredient"), SerializeField] public float _quantity;


        [BoxGroup("Ingredient"), SerializeField, PreviewField, Tooltip("Sprite used as the ingredient picked up from the dispenser")] 
        private Sprite _ingredient;
    }
}