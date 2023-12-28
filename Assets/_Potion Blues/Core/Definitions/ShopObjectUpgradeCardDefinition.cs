using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Upgrades/Shop Object")]
    public class ShopObjectUpgradeCardDefintion : UpgradeCardDefinition
    {
        public ShopObjectDefinition ShopObject => _shopObject;

        [BoxGroup("Shop Object"), SerializeField, ValueDropdown("GetObjects")] private ShopObjectDefinition _shopObject;

#if UNITY_EDITOR
        private static IEnumerable GetObjects()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:ShopObjectDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<ShopObjectDefinition>(x))
                .Select(x => new ValueDropdownItem($"{x.Category.name}/{x.name}", x));
        }
#endif
    }
}
