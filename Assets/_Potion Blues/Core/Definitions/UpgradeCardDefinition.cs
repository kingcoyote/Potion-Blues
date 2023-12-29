using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.Collections;

namespace PotionBlues.Definitions
{
    public class UpgradeCardDefinition : ScriptableObject
    {
        public UpgradeCardTypeDefinition Type => _type;
        public string Description => _description;
        public int Priority => _priority;
        public Sprite Icon => _icon;
        public Sprite Picture => _picture;
        public int ReputationCost => _reputationCost;
        public int GoldCost => _goldCost;

        [BoxGroup("Base"), SerializeField, ValueDropdown("GetUpgradeCardTypes")] 
        private UpgradeCardTypeDefinition _type;
        [BoxGroup("Base"), SerializeField, TextArea(3, 10), HideIf("@_type.name == \"Object\"")] 
        private string _description;
        [BoxGroup("Base"), SerializeField, Tooltip("The order that upgrades are processed, from lowest to highest")] 
        private int _priority;
        [BoxGroup("Base"), SerializeField, PreviewField, Tooltip("Small picture that will be shown during a game to remind players what upgrades they have")] 
        private Sprite _icon;
        [BoxGroup("Base"), SerializeField, PreviewField, Tooltip("Large picture that will be shown on the card when selecting cards")] 
        private Sprite _picture;
        [BoxGroup("Base"), SerializeField] 
        private int _reputationCost;
        [BoxGroup("Base"), SerializeField] 
        private int _goldCost;

#if UNITY_EDITOR
        private static IEnumerable GetUpgradeCardTypes()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:UpgradeCardTypeDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<UpgradeCardTypeDefinition>(x))
                .Select(x => new ValueDropdownItem(x.name, x));
        }
#endif
    }
}
