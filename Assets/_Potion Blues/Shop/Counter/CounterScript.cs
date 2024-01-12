using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class CounterScript : ShopObjectScript
    {
        [BoxGroup("Base"), OnValueChanged("StartCounter")]
        public CounterDefinition Counter;

        [BoxGroup("Instance")] public float ShelfLife;
        [BoxGroup("Instance")] public int CounterSlots;
        [BoxGroup("Instance")] public int SlotCapacity;

        new public void Start()
        {
            base.Start();
            Debug.Log($"Starting counter script {name}");

            _bus.SubscribeToTarget<CounterEvent>(this, OnCounterEvent);

            StartCounter();
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<CounterEvent>(this, OnCounterEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void LoadShopObject(ShopObjectDefinition definition)
        {
            if (definition.GetType() != typeof(CounterDefinition))
            {
                throw new ArgumentException($"CounterScript cannot load an object of type {definition.GetType()}");
            }

            Counter = (CounterDefinition)definition;
        }

        public void StartCounter()
        {
            var attrs = new List<ShopAttributeValue>()
            {
                new ShopAttributeValue("Shelf Life", Counter.ShelfLife),
                new ShopAttributeValue("Counter Capacity", Counter.SlotCapacity),
                new ShopAttributeValue("Counter Slots", Counter.CounterSlots),
            };

            _bus.Raise(new CounterEvent(CounterEventType.Spawn, attrs), this, this);
        }

        void OnCounterEvent(ref CounterEvent evt, IEventNode target, IEventNode source)
        {
            switch (evt.Type)
            {
                case CounterEventType.Spawn:
                    ShelfLife = evt.Attributes.Find(a => a.Attribute.name == "Shelf Life").Value;
                    SlotCapacity = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Capacity").Value;
                    CounterSlots = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Slots").Value;
                    break;
            }
        }
    }
}