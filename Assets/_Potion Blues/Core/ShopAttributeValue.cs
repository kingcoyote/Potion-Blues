using PotionBlues.Definitions;
using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PotionBlues
{
    [Serializable]
    public struct ShopAttributeValue
    {
        [ValueDropdown("GetAttributes")]
        public ShopAttributeDefinition Attribute;
        public float Value;

        public ShopAttributeValue(ShopAttributeDefinition def, float val)
        { 
            Attribute = def; 
            Value = val; 
        }

        public ShopAttributeValue(string name, float val)
        {
            Attribute = Resources.Load<ShopAttributeDefinition>($"Shop Attributes/{name}");
            Value = val;
        }

        public override string ToString()
        {
            return $"{Attribute.name} = {Value}";
        }

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