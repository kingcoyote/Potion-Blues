using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace PotionBlues.Shop
{
    public class CauldronScript : ShopObjectScript
    {
        [OnValueChanged("StartCauldron")]
        public CauldronDefinition Cauldron;

        public float BrewingTime;
        public float BrewingOutput;

        // Use this for initialization
        new public void Start()
        {
            base.Start();
            Debug.Log($"Starting cauldron script {name}");

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);

            StartCauldron();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void LoadShopObject(ShopObjectDefinition definition)
        {
            if (definition.GetType() != typeof(CauldronDefinition))
            {
                throw new ArgumentException($"CauldronScript cannot load an object of type {definition.GetType()}");
            }

            Cauldron = (CauldronDefinition)definition;
            
        }

        public void StartCauldron()
        {
            var attrs = new List<ShopAttributeValue>()
            {
                new ShopAttributeValue("Brewing Time", Cauldron.BrewingTime),
                new ShopAttributeValue("Brewing Output", Cauldron.BrewingOutput),
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
                    break;
            }
        }
    }
}