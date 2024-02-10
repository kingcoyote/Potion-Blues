using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class DoorScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public float CustomerFrequency;
        [BoxGroup("Instance")] public float CustomerPatience;
        [BoxGroup("Instance")] public float CustomerTipping;
        [BoxGroup("Instance")] public float ReputationBonus;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<DoorEvent>(this, OnDoorEvent);
            _bus.Raise(new DoorEvent(DoorEventType.Spawn, Definition.Attributes), this, this);
        }

        public void OnDestroy()
        {
            if (_bus != null )
                _bus.UnsubscribeFromTarget<DoorEvent>(this, OnDoorEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {

            switch (evt.Type)
            {
                case DoorEventType.Spawn:
                    CustomerFrequency = evt.Attributes.Find(a => a.Attribute.name == "Customer Frequency").Value;
                    CustomerPatience = evt.Attributes.Find(a => a.Attribute.name == "Customer Patience").Value;
                    CustomerTipping = evt.Attributes.Find(a => a.Attribute.name == "Customer Tipping").Value;
                    ReputationBonus = evt.Attributes.Find(a => a.Attribute.name == "Reputation Bonus").Value;
                    break;
            }
        }
    }
}