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
        [BoxGroup("Base"), OnValueChanged("StartCauldron")]
        public CauldronDefinition Cauldron;

        [BoxGroup("Instance")] public float BrewingTime;
        [BoxGroup("Instance")] public float BrewingOutput;
        [BoxGroup("Instance")] public float CleaningInterval;
        [BoxGroup("Instance")] public float FailureRate;

        // Use this for initialization
        new public void Start()
        {
            base.Start();
            Debug.Log($"Starting cauldron script {name}");

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);

            StartCauldron();
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<CauldronEvent>(this, OnCauldronEvent);
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
            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, Cauldron.Attributes), this, this);
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