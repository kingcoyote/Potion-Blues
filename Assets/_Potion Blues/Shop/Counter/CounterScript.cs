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
        [BoxGroup("Instance")] public float ShelfLife;
        [BoxGroup("Instance")] public int CounterSlots;
        [BoxGroup("Instance")] public int SlotCapacity;

        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CounterEvent>(this, OnCounterEvent);
            _bus.Raise(new CounterEvent(CounterEventType.Spawn, Definition.Attributes), this, this);
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