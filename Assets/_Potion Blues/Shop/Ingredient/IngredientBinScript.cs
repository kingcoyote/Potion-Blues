using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using VectorSwizzling;
using PotionBlues.Definitions;
using PotionBlues.Events;

namespace PotionBlues.Shop
{
    public class IngredientBinScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public float IngredientCost;
        [BoxGroup("Instance")] public float IngredientSalvage;
        [BoxGroup("Instance")] public float IngredientCooldown;
        [BoxGroup("Instance")] public int IngredientQuantity;

        [BoxGroup("Ingredients")] public IngredientScript IngredientPrefab;

        public int Quantity;

        private PlayerInput _input;
        private BoxCollider2D _box;

        private IngredientDefinition _ingredient => (IngredientDefinition)Definition;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<IngredientEvent>(this, OnIngredientEvent);
            _bus.Raise(new IngredientEvent(IngredientEventType.Spawn, Definition.Attributes), this, this);
            
            _box = GetComponent<BoxCollider2D>();

            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].performed += OnSelect;
            _input.actions["Select"].canceled += OnDeselect;
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

        public void OnSelect(InputAction.CallbackContext _context)
        {
            var cursor = Camera.main.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            if (_box.bounds.Contains(cursor))
            {
                _bus.Raise(
                    new IngredientEvent(IngredientEventType.Select, Definition.Attributes) { 
                        Definition = _ingredient
                    },
                    this,
                    this
                );
            }
        }

        public void SpawnIngredient(IngredientDefinition ingredient, List<ShopAttributeValue> attributes)
        {
            var go = Instantiate(IngredientPrefab);
            go.Ingredient = ingredient;
            go.IngredientBin = this;
            go.Attributes = attributes;

            Quantity -= 1;
        }

        public void OnDeselect(InputAction.CallbackContext _context)
        {
            
        }

        void OnIngredientEvent(ref IngredientEvent evt, IEventNode target, IEventNode source)
        {
            IngredientCost = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Cost").Value;
            IngredientSalvage = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Salvage").Value;
            IngredientCooldown = evt.Attributes.Find(a => a.Attribute.name == "Ingredient Cooldown").Value;
            IngredientQuantity = (int)evt.Attributes.Find(a => a.Attribute.name == "Ingredient Quantity").Value;

            switch (evt.Type)
            {
                case IngredientEventType.Spawn:
                    Quantity = IngredientQuantity;
                    break;
                case IngredientEventType.Select:
                    if (target != this) return;
                    if (Quantity <= 0) return;
                    SpawnIngredient(evt.Definition, evt.Attributes);
                    break;
            }
        }
    }
}