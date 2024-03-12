using GenericEventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class QueueScript : MonoBehaviour
    {
        [SerializeField] private ShopObjectContainerScript _counterContainer;

        private List<CounterScript> _counters;
        private GenericEventBus<IEvent, IEventNode> _bus;

        public void Start()
        {
            _bus = PotionBlues.I().EventBus;
            _bus.SubscribeTo<RunEvent>(OnRunEvent, -1000);
        }

        public void OnDestroy()
        {
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
        }

        private void OnRunEvent(ref RunEvent evt)
        {
            switch (evt.Type)
            {
                case RunEventType.DayStarted:
                    _counters = _counterContainer.GetComponentsInChildren<CounterScript>().ToList();
                    break;
            }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            var customer = collision.gameObject.GetComponent<CustomerScript>();
            if (customer != null)
            {
                ServeCustomer(customer);
            }
        }

        private void ServeCustomer(CustomerScript customer)
        {
            var slots = _counters.SelectMany(counter => counter.Slots);

            foreach (var slot in slots)
            {
                if (slot.PotionType != customer.DesiredPotion || slot.Potions.Count == 0) continue;
                    
                var potion = slot.Potions.Dequeue();
                customer.BuyPotion(potion);
            }
        }
    }
}