using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using PotionBlues.Definitions;
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

    public static class ShopAttributeValueUtilities
    {
        public static List<ShopAttributeValue> Stack(this List<ShopAttributeValue> left, List<ShopAttributeValue> right)
        {
            var keys = left.Select(av => av.Attribute).Union(right.Select(av => av.Attribute));
            var leftDict = left.ToDictionary();
            var rightDict = right.ToDictionary();

            return keys
                .Select(k => new ShopAttributeValue(k, k.Stack(leftDict.TryGet(k), rightDict.TryGet(k))))
                .ToList();
        }

        public static Dictionary<ShopAttributeDefinition, float> ToDictionary(this List<ShopAttributeValue> input)
        {
            return input
                .GroupBy(av => av.Attribute)
                .Select(group => new ShopAttributeValue(
                    group.Key,
                    group.Select(av => av.Value).Aggregate((a, b) => group.Key.Stack(a, b))
                ))
                .ToDictionary(av => av.Attribute, av=> av.Value);
        }

        public static float TryGet(this Dictionary<ShopAttributeDefinition, float> input, ShopAttributeDefinition key)
        {
            return input.ContainsKey(key) ? input[key] : key.Identity;
        }

        public static float TryGet(this List<ShopAttributeValue> input, string key)
        {
            return input.ToDictionary().TryGet(PotionBlues.I().ShopAttributeDefinitions[key]);
        }
    }
}