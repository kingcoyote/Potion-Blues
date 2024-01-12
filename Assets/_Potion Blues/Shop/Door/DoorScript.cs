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
        [BoxGroup("Base"), OnValueChanged("StartDoor")]
        public DoorDefinition Door;

        [BoxGroup("Instance")] public float CustomerFrequency;
        [BoxGroup("Instance")] public float CustomerPatience;
        [BoxGroup("Instance")] public float CustomerTipping;
        [BoxGroup("Instance")] public float ReputationBonus;


        // Use this for initialization
        new public void Start()
        {
            base.Start();
            Debug.Log($"Starting door script {name}");

            _bus.SubscribeToTarget<DoorEvent>(this, OnDoorEvent);

            StartDoor();
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

        protected override void LoadShopObject(ShopObjectDefinition definition)
        {
            if (definition.GetType() != typeof(DoorDefinition))
            {
                throw new ArgumentException($"DoorScript cannot load an object of type {definition.GetType()}");
            }

            Door = (DoorDefinition)definition;
        }

        public void StartDoor()
        {
            var attrs = new List<ShopAttributeValue>()
            {
                new ShopAttributeValue("Customer Frequency", Door.CustomerFrequency),
                new ShopAttributeValue("Customer Patience", Door.CustomerPatience),
                new ShopAttributeValue("Customer Tipping", Door.CustomerTipping),
                new ShopAttributeValue("Reputation Bonus", Door.ReputationBonus),
            };

            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, attrs), this, this);
        }

        void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {
            Debug.Log($"Door is reacting to door event from {source}");

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