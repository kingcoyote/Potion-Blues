using UnityEngine;
using PotionBlues.Definitions;
using GenericEventBus;

namespace PotionBlues.Shop
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ShopObjectScript : MonoBehaviour, IEventNode
    {
        protected GenericEventBus<IEvent, IEventNode> _bus;
        private SpriteRenderer _sprite;

        protected abstract void LoadShopObject(ShopObjectDefinition definition);

        // Use this for initialization
        public void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
            LoadBus();

            Debug.Log($"Starting shop object script - {name}");
        }

        protected void LoadBus()
        {
            _bus = PotionBlues.I().EventBus;
        }

        public void LoadDefinition(ShopObjectDefinition definition)
        {
            LoadSprite(definition);
            LoadShopObject(definition);
        }

        void LoadSprite(ShopObjectDefinition definition)
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.sprite = definition.Sprite;
        }

    }
}