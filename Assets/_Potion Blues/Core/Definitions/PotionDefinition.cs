using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Potion")]
    public class PotionDefinition : ScriptableObject
    {
        public string Description => _description;
        public int Value => _value;
        public Sprite Icon => _icon;
        public Sprite Potion => _potion;
        public Color Color => _color;
        public List<IngredientDefinition> Ingredients => _ingredients;

        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField] private int _value;
        [SerializeField, PreviewField] private Sprite _icon;
        [SerializeField, PreviewField] private Sprite _potion;
        [SerializeField] private Color _color;
        [SerializeField, ValueDropdown("GetIngredients")] private List<IngredientDefinition> _ingredients;

#if UNITY_EDITOR
        private static IEnumerable GetIngredients()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:IngredientDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<IngredientDefinition>(x))
                .Where(x => x.Enhancer == false)
                .Select(x => new ValueDropdownItem($"{x.name}", x));
        }
#endif
    }
}