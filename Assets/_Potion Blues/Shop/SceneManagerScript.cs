using GenericEventBus;
using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop {
    public class SceneManagerScript : MonoBehaviour
    {
        [SerializeField] public RunData CurrentRun;
        [SerializeField] public ShopObjectContainerScript Doors;
        [SerializeField] public ShopObjectContainerScript Counters;
        [SerializeField] public ShopObjectContainerScript Cauldrons;
        [SerializeField] public ShopObjectContainerScript Ingredients;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            _bus = PotionBlues.I().EventBus;
            _bus.SubscribeTo<ShopObjectEvent>(OnShopObjectEvent);
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<ShopObjectEvent>(OnShopObjectEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button]
        public void InstantiateShopObjects()
        {
            var categories = PotionBlues.I().ShopObjectCategories;

            Doors.ResetShopObjects(CurrentRun.GetShopObjects(categories["Door"]));
            Counters.ResetShopObjects(CurrentRun.GetShopObjects(categories["Counter"]));
            Cauldrons.ResetShopObjects(CurrentRun.GetShopObjects(categories["Cauldron"]));
            Ingredients.ResetShopObjects(CurrentRun.GetShopObjects(categories["Ingredient"]));
        }

        public void OnShopObjectEvent(ref ShopObjectEvent evt)
        {
            Debug.Log($"Scene manager is reacting to shop object event");
        }
    }
}
