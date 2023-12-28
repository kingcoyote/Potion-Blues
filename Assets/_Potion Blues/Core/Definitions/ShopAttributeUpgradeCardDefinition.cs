using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Upgrades/Shop Attribute")]
    public class ShopAttributeUpgradeCardDefintion : UpgradeCardDefinition
    {
        public List<ShopAttributeValue> AttributeValues => _attributeValues;

        [BoxGroup("Shop Attribute"), TableList, SerializeField] public List<ShopAttributeValue> _attributeValues;
    }
}
