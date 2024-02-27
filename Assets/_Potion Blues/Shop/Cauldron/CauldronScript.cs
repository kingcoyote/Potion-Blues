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
        [BoxGroup("Instance")] public List<ShopAttributeValue> Attributes;

        [BoxGroup("Potion")] public PotionScript Potion;
        [BoxGroup("Potion")] public PotionScript PotionPrefab;

        public int RemainingPotions;

        public float BrewTime;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);
            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, Definition.Attributes), this, this);

            Potion = Instantiate(PotionPrefab, transform);
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<CauldronEvent>(this, OnCauldronEvent);
        }

        // Update is called once per frame
        void Update()
        {
            if (Potion.State != PotionScript.PotionState.Brewing) return;
            BrewTime -= Time.deltaTime;
            if (BrewTime < 0)
            {
                FinishBrewing();
            }
        }

        [Button]
        public void StartBrewing()
        {
            if (Potion.State != PotionScript.PotionState.Mixed) return;
            Potion.Attributes = Potion.Attributes.Stack(Attributes);
            Potion.State = PotionScript.PotionState.Brewing;
            BrewTime = Potion.Attributes.TryGet("Brewing Time");
            // if potion is ready to be brewed
        }

        public void FinishBrewing()
        {
            Potion.State = PotionScript.PotionState.Ready;
            _bus.Raise(new CauldronEvent(CauldronEventType.BrewFinish, Potion.Attributes));

            RemainingPotions -= 1;
            var quantity = Potion.Attributes.TryGet("Brewing Output");
            var failure = Potion.Attributes.TryGet("Failure Rate");
            var rng = PotionBlues.I().RNG;

            if (failure > rng.NextFloat())
            {
                Debug.Log("Potion failed. Starting over");
                Destroy(Potion.gameObject);
                Potion = Instantiate(PotionPrefab, transform);
                _bus.Raise(new CauldronEvent(CauldronEventType.BrewFailed, Potion.Attributes));
            }

            quantity = quantity % 1 < rng.NextFloat() ? Mathf.Floor(quantity) : Mathf.Ceil(quantity);
            Potion.Attributes.Set("Brewing Output", quantity);
        }

        void OnCauldronEvent(ref CauldronEvent evt, IEventNode target, IEventNode source)
        {
            switch (evt.Type)
            {
                case CauldronEventType.Spawn:
                    Attributes = evt.Attributes;
                    RemainingPotions = (int)Attributes.TryGet("Cleaning Interval");
                    break;
                case CauldronEventType.IngredientAdd:
                    if (RemainingPotions <= 0)
                    {
                        Debug.Log("Cannot add ingredients to dirty cauldron");
                        return;
                    }
                    if (Potion.AddIngredient(evt.Ingredient))
                    {
                        Potion.Attributes = Potion.Attributes.Stack(evt.Attributes);
                        _bus.ConsumeCurrentEvent();
                    } else
                    {
                        Debug.LogError("Duplicate ingredient added to potion - inventory consumed incorrectly. FIXME");
                    }
                    // copy the attributes into the potion script being staged
                    // if there is a potion that
                    break;
            }
        }
    }
}