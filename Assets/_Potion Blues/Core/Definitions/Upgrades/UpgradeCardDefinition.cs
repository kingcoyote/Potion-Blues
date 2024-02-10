using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.Collections;

namespace PotionBlues.Definitions
{
    public class UpgradeCardDefinition : ScriptableObject
    {
        public string Reference => _reference;
        public string Description => _description;
        public int Priority => _priority;
        public Sprite Icon => _icon;
        public Sprite Picture => _picture;
        public int ReputationCost => _reputationCost;
        public int GoldCost => _goldCost;
        public RarityDefinition Rarity => _rarity;
        public bool Unique => _unique;

        [BoxGroup("Base"), SerializeField]
        private string _reference;
        [BoxGroup("Base"), SerializeField, TextArea(3, 10)] 
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
        [BoxGroup("Base"), SerializeField, ValueDropdown("@RarityDefinition.GetRarity()")]
        private RarityDefinition _rarity;
        [BoxGroup("Base"), SerializeField]
        private bool _unique;
    }
}
