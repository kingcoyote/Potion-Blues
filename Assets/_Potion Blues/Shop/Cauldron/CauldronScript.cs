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
        [BoxGroup("Instance")] public float BrewingTime;
        [BoxGroup("Instance")] public float BrewingOutput;
        [BoxGroup("Instance")] public float CleaningInterval;
        [BoxGroup("Instance")] public float FailureRate;

        [BoxGroup("Potion")] public PotionScript Potion;
        [BoxGroup("Potion")] public PotionScript PotionPrefab;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);
            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, Definition.Attributes), this, this);

            Potion = Instantiate(PotionPrefab);
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

        void OnCauldronEvent(ref CauldronEvent evt, IEventNode target, IEventNode source)
        {
            switch (evt.Type)
            {
                case CauldronEventType.Spawn:
                    BrewingTime = evt.Attributes.Find(a => a.Attribute.name == "Brewing Time").Value;
                    BrewingOutput = evt.Attributes.Find(a => a.Attribute.name == "Brewing Output").Value;
                    CleaningInterval = evt.Attributes.Find(a => a.Attribute.name == "Cleaning Interval").Value;
                    FailureRate = evt.Attributes.Find(a => a.Attribute.name == "Failure Rate").Value;
                    break;
                case CauldronEventType.IngredientAdd:
                    // copy the attributes into the potion script being staged
                    // if there is a potion that
                    break;
            }
        }
    }
}