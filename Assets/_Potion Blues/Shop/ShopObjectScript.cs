using UnityEngine;
using PotionBlues.Definitions;
using GenericEventBus;

namespace PotionBlues.Shop
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ShopObjectScript : MonoBehaviour
    {
        protected GenericEventBus<IEvent, IEventNode> _bus;
        private SpriteRenderer _sprite;

        protected abstract void LoadShopObject(ShopObjectDefinition definition);

        // Use this for initialization
        void Start()
        {
            _bus = PotionBlues.I().EventBus;
            _sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

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