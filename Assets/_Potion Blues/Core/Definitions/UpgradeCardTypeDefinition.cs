using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Upgrades/Type" +
        "")]
    public class UpgradeCardTypeDefinition : ScriptableObject
    {
        public Sprite Frame => _frame;
        public Color Color => _color;
        public string Description => _description;

        [SerializeField, PreviewField, Tooltip("A 9-slice sprite that will be used for the upgrade card border")] 
        private Sprite _frame;
        [SerializeField, Tooltip("Background color used for upgrade cards of this type")] 
        private Color _color;
        [SerializeField, TextArea(3, 10)] 
        private string _description;
    }
}
