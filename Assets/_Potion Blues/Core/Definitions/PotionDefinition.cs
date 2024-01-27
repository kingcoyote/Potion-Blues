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

        [SerializeField, TextArea(3, 10)] 
        private string _description;
        [SerializeField, Tooltip("How much gold this potion is worth")] 
        private int _value;
        [SerializeField, PreviewField, Tooltip("Icon that will be shown above customers or the counter to symbolize this potion")] 
        private Sprite _icon;
        [SerializeField, PreviewField, Tooltip("Sprite used for rendering this potion when moving it around or placing it on the counter")] 
        private Sprite _potion;
        [SerializeField, Tooltip("Color used for the icon or for customers requesting this potion")] 
        private Color _color;
        [SerializeField, ValueDropdown("GetIngredients"), Tooltip("Ingredients needed to make this potion")] 
        private List<IngredientDefinition> _ingredients;

#if UNITY_EDITOR
        private static IEnumerable GetIngredients()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:IngredientDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<IngredientDefinition>(x))
                .Where(x => x.Attributes.Count() == 0)
                .Select(x => new ValueDropdownItem($"{x.name}", x));
        }
#endif
    }
}