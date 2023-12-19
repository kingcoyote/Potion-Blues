using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBlues.Prototypes.Autodispense
{
    [CreateAssetMenu(menuName = "Potion Blues/Prototypes/Auto Dispense/Potion Attribute")]
    public class PotionAttributeDefinition : ScriptableObject
    {
        public Sprite Icon => _icon;
        public Color Color => _color;
        public Sprite Potion => _potion;
        public Sprite Ingredient => _ingredient;
        public string Description => _description;

        [SerializeField, PreviewField()] private Sprite _icon;
        [SerializeField] private Color _color;
        [SerializeField, PreviewField()] private Sprite _potion;
        [SerializeField, PreviewField()] private Sprite _ingredient;
        [SerializeField, TextArea(3, 10)] private string _description;
    }
}