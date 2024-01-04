using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Upgrades/Daily")]
    public class DailyUpgradeDefinition : UpgradeCardDefinition
    {
        public List<ShopAttributeValue> AttributeValues => _attributeValues;

        [BoxGroup("Shop Attribute"), TableList, SerializeField] public List<ShopAttributeValue> _attributeValues;
    }
}