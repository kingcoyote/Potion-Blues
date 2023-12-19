using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Potion Type")]
    public class PotionTypeDefinition : ScriptableObject
    {
        public bool Active => _active;
        public string Description => _description;
        public Sprite Icon => _icon;
        public Sprite Ingredient => _ingredient;
        public string IngredientName => _ingredientName;
        public Sprite Potion => _potion;
        public string PotionName => _potionName;
        public Color Color => _color;

        [SerializeField] private bool _active;
        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _ingredient;
        [SerializeField] private string _ingredientName;
        [SerializeField] private Sprite _potion;
        [SerializeField] private string _potionName;
        [SerializeField] private Color _color;
    }
}