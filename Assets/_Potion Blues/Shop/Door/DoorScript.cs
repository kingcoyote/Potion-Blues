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

        void OnCauldronEvent(ref CauldronEvent evt, IEventNode target, IEventNode source)
        {
            Debug.Log($"Cauldron is reacting to cauldron event from {source}");

            switch (evt.Type)
            {
                case CauldronEventType.Spawn:
                    BrewingTime = evt.Attributes.Find(a => a.Attribute.name == "Brewing Time").Value;
                    BrewingOutput = evt.Attributes.Find(a => a.Attribute.name == "Brewing Output").Value;
                    CleaningInterval = evt.Attributes.Find(a => a.Attribute.name == "Cleaning Interval").Value;
                    FailureRate = evt.Attributes.Find(a => a.Attribute.name == "Failure Rate").Value;
                    break;
            }
        }
    }
}