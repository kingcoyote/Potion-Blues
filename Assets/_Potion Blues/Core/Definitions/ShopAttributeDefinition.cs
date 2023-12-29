using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Attribute")]
    public class ShopAttributeDefinition : ScriptableObject
    {
        public string Description => _description;
        public Sprite Icon => _icon;
        public StackType StackingType => _stackingType;

        [SerializeField, TextArea(3,10)] private string _description;
        [SerializeField, PreviewField] private Sprite _icon;
        [SerializeField] private StackType _stackingType;
    }

    public enum StackType
    {
        Multiply, // stack by multiplying attribute values together, e.g. 1.1 stacked with 1.1 = 1.21.
        Add, // stack by adding attribute values together, e.g. 3 stacked with 1 = 4
        Overwrite, // stack by replacing initial value with subsequent value, e.g. 3 stacked with 1 = 1
        Ignore // stack by ignoring subsequent value and keeping initial value, e.g. 3 stacked with 1 = 3
    }
}