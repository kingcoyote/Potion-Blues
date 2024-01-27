using System.Collections;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace PotionBlues.Definitions
{
    public abstract class ShopObjectDefinition : ScriptableObject
    {
        public ShopObjectCategoryDefinition Category => _category;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public List<ShopAttributeValue> Attributes => _attributes;

        [BoxGroup("Object"), SerializeField, ValueDropdown("@ShopObjectCategoryDefinition.GetCategories()")] private ShopObjectCategoryDefinition _category;
        [BoxGroup("Object"), SerializeField, TextArea(3, 10)] private string _description;
        [BoxGroup("Object"), SerializeField, PreviewField, Tooltip("Sprite that will be displayed in game for this object")] private Sprite _sprite;

        [BoxGroup("Attributes"), SerializeField, TableList] private List<ShopAttributeValue> _attributes;
    }
}