using GenericEventBus;
using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop {
    public class SceneManagerScript : MonoBehaviour
    {
        [SerializeField] public ShopObjectContainerScript Doors;
        [SerializeField] public ShopObjectContainerScript Counters;
        [SerializeField] public ShopObjectContainerScript Cauldrons;
        [SerializeField] public ShopObjectContainerScript Ingredients;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            _bus = PotionBlues.I().EventBus;
            _bus.SubscribeTo<DoorEvent>(OnDoorEvent, 100);
            _bus.SubscribeTo<CounterEvent>(OnCounterEvent, 100);
            _bus.SubscribeTo<CauldronEvent>(OnCauldronEvent, 100);
            _bus.SubscribeTo<IngredientEvent>(OnIngredientEvent, 100);

            ClearShopObjects();
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<DoorEvent>(OnDoorEvent);
            _bus.UnsubscribeFrom<CounterEvent>(OnCounterEvent);
            _bus.UnsubscribeFrom<CauldronEvent>(OnCauldronEvent);
            _bus.UnsubscribeFrom<IngredientEvent>(OnIngredientEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button]
        public void InstantiateShopObjects()
        {
            var categories = PotionBlues.I().ShopObjectCategories;

            var activeRun = PotionBlues.I().GameData.ActiveRun;

            Doors.ResetShopObjects(activeRun.GetShopObjects(categories["Door"]));
            Counters.ResetShopObjects(activeRun.GetShopObjects(categories["Counter"]));
            Cauldrons.ResetShopObjects(activeRun.GetShopObjects(categories["Cauldron"]));
            Ingredients.ResetShopObjects(activeRun.GetShopObjects(categories["Ingredient"]));
        }

        [Button]
        public void ClearShopObjects()
        {
            Doors.ClearShopObjects();
            Counters.ClearShopObjects();
            Cauldrons.ClearShopObjects();
            Ingredients.ClearShopObjects();
        }

        public void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        public void OnCounterEvent(ref CounterEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        public void OnCauldronEvent(ref CauldronEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        public void OnIngredientEvent(ref IngredientEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        private List<ShopAttributeValue> AdjustAttributes(List<ShopAttributeValue> attributes)
        {
            var adjustedAttributes = new List<ShopAttributeValue>();
            var upgradeModifiers = GetAttributeModifiers();

            foreach(var attr in attributes)
            {
                float modifier = upgradeModifiers.TryGetValue(attr.Attribute, out modifier) ? modifier : 1;
                adjustedAttributes.Add(new ShopAttributeValue(attr.Attribute, attr.Attribute.Aggregate(attr.Value, modifier)));
            }

            return adjustedAttributes;
        }

        /**
         * Process the current run's shop attribute upgrades to return a flattened dict of each attribute
         * and the total value, either a sum or product.
         */
        private Dictionary<ShopAttributeDefinition, float> GetAttributeModifiers()
        {
            return PotionBlues.I().GameData.ActiveRun
                .GetShopAttributeUpgrades()
                .SelectMany(card => card.AttributeValues)
                .GroupBy(av => av.Attribute)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Key.Aggregate(group.ToList()) // group.Select(a => a.Value).Aggregate((a, b) => a * b)
                );
        }
    }
}
