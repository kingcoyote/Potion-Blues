using UnityEngine;
using PotionBlues.Definitions;
using GenericEventBus;

namespace PotionBlues.Shop
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ShopObjectScript : MonoBehaviour, IEventNode
    {
        protected ShopObjectDefinition Definition;
        protected GenericEventBus<IEvent, IEventNode> _bus;

        public void Start()
        {
            _bus = PotionBlues.I().EventBus;
        }

        public void SetDefinition(ShopObjectDefinition definition)
        {
            name = definition.name;
            GetComponent<SpriteRenderer>().sprite = definition.Sprite;

            Definition = definition;
        }
    }
}