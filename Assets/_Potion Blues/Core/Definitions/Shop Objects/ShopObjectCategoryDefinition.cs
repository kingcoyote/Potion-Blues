using System.Linq;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Object Category")]
    public class ShopObjectCategoryDefinition : ScriptableObject
    {
        public string Description => _description;
        public int Max => _max;

        [SerializeField, TextArea(3,10)] private string _description;
        [SerializeField] private int _max;

        public static ShopObjectCategoryDefinition Load(string name)
        {
            return Resources.Load<ShopObjectCategoryDefinition>($"Object Categories/{name}");
        }

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