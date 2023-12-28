using PotionBlues.Definitions;
using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

namespace PotionBlues
{
    [Serializable]
    public struct ShopAttributeValue
    {
        [ValueDropdown("GetAttributes")]
        public ShopAttributeDefinition Attribute;
        public float Value;

#if UNITY_EDITOR
        private static IEnumerable GetAttributes()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:ShopAttributeDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<ShopAttributeDefinition>(x))
                .Select(x => new ValueDropdownItem(x.name, x));
        }
#endif
    }
}