using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class IngredientScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public float IngredientCost;
        [BoxGroup("Instance")] public float IngredientSalvage;
        [BoxGroup("Instance")] public float IngredientCooldown;
        [BoxGroup("Instance")] public int IngredientQuantity;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<IngredientEvent>(this, OnIngredientEvent);
            _bus.Raise(new IngredientEvent(IngredientEventType.Spawn, Definition.Attributes), this, this);
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<IngredientEvent>(this, OnIngredientEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnIngredientEvent(ref IngredientEvent evt, IEventNode target, IEventNode source)
        {
            switch (evt.Type)
            {
                case IngredientEventType.Spawn:
                    IngredientCost = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Cost").Value;
                    IngredientSalvage = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Salvage").Value;
                    IngredientCooldown = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Cooldown").Value;
                    IngredientQuantity = (int)evt.Attributes.Find(a => a.Attribute.name == "Ingredient Quantity").Value;
                    break;
            }
        }
    }
}