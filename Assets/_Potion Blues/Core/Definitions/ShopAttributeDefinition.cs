using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Attribute")]
    public class ShopAttributeDefinition : ScriptableObject
    {
        public string Description => _description;
        public Sprite Icon => _icon;

        [SerializeField, TextArea(3,10)] private string _description;
        [SerializeField, PreviewField] private Sprite _icon;
    }
}