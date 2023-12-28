using System.Collections;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Object")]
    public abstract class ShopObjectDefinition : ScriptableObject
    {
        public ShopObjectCategoryDefinition Category => _category;
        public string Description => _description;

        [BoxGroup("Object"), SerializeField, ValueDropdown("GetCategories")] private ShopObjectCategoryDefinition _category;
        [BoxGroup("Object"), SerializeField, TextArea(3, 10)] private string _description;

#if UNITY_EDITOR
        private static IEnumerable GetCategories()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:ShopObjectCategoryDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<ShopObjectCategoryDefinition>(x))
                .Select(x => new ValueDropdownItem(x.name, x));
        }
#endif
    }
}