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

        [SerializeField] private Sprite _frame;
        [SerializeField] private Color _color;
        [SerializeField, TextArea(3, 10)] private string _description;
    }
}
