using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Ingredient")]
    public class IngredientDefinition : ShopObjectDefinition
    {
        public float Cost => _cost;
        public Sprite Ingredient => _ingredient;
        public bool Enhancer => _enhancer;
        public List<ShopAttributeValue> Attributes => _attributes;

        [BoxGroup("Ingredient"), SerializeField] public float _cost;
        [BoxGroup("Ingredient"), SerializeField, PreviewField, Tooltip("Sprite used as the ingredient picked up from the dispenser")] 
        private Sprite _ingredient;
        [BoxGroup("Ingredient"), SerializeField] private bool _enhancer;
        [BoxGroup("Ingredient"), SerializeField, ShowIf("_enhancer"), TableList] private List<ShopAttributeValue> _attributes;
    }
}