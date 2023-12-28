using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Linq;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Upgrades/Ingredient")]
    public class IngredientUpgradeCardDefintion : UpgradeCardDefinition
    {
        public IngredientDefinition Ingredient => _ingredient;

        [BoxGroup("Ingredient"), SerializeField, ValueDropdown("GetIngredients")] private IngredientDefinition _ingredient;

#if UNITY_EDITOR
        private static IEnumerable GetIngredients()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:IngredientDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<IngredientDefinition>(x))
                .Select(x => new ValueDropdownItem($"{(x.Enhancer ? "Enhancer" : "Ingredient")}/{x.name}", x));
        }
#endif
    }
}
