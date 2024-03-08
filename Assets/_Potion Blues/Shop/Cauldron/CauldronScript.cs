using PotionBlues.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using VectorSwizzling;
using System;
using PotionBlues.Definitions;

namespace PotionBlues.Shop
{
    public class CauldronScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public List<ShopAttributeValue> Attributes;

        [BoxGroup("Potion")] public PotionScript Potion;
        [BoxGroup("Potion")] public PotionScript PotionPrefab;

        public int RemainingPotions;
        public float BrewTime;

        [SerializeField] public SpriteRenderer _fill;
        [SerializeField] public ParticleSystem _bubbles;

        private PlayerInput _input;
        private BoxCollider2D _box;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);
            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, Definition.Attributes), this, this);

            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].performed += OnSelect;

            _box = GetComponent<BoxCollider2D>();

            var sprite = GetComponent<SpriteRenderer>().sprite;
            var fillOffset = (sprite.textureRect.height / sprite.pixelsPerUnit) / 2;
            _fill.sprite = ((CauldronDefinition)Definition).Fill;
            _fill.transform.localPosition = Vector3.up * fillOffset;
            _fill.enabled = false;
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<CauldronEvent>(this, OnCauldronEvent);

            if (_input != null)
            {
                _input.actions["Select"].performed -= OnSelect;
            }
        }

        public void OnSelect(InputAction.CallbackContext _context)
        {
            var cursor = Camera.main.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            if (_box.bounds.Contains(cursor))
            {
                switch (Potion.State)
                {
                    case PotionScript.PotionState.Mixed:
                        StartBrewing();
                        break;
                    case PotionScript.PotionState.Ready:
                        _bus.Raise(new CauldronEvent(CauldronEventType.PotionRemove, Potion.Attributes), this, this);
                        break;
                }
            }
        }

        private void SpawnPotion()
        {
            Debug.Log("Spawning potions");
            Potion.Select();
            Potion.Show();
            Potion = null;
        }

        // Update is called once per frame
        void Update()
        {
            if (Potion == null)
            {
                _fill.enabled = false;
                return;
            }
            
            _fill.color = Potion.Definition == null ? Color.cyan : Potion.Definition.Color;
            _fill.enabled = true;

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
            var bubbleProperties = _bubbles.main;
            bubbleProperties.duration = BrewTime;
            // bubbleProperties.startColor = Potion.Definition.Color;
            _bubbles.Play();
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

                    if (Potion == null)
                    {
                        Potion = Instantiate(PotionPrefab, transform);
                        Potion.Cauldron = this;
                    }

                    if (Potion.AddIngredient(evt.Ingredient))
                    {
                        Potion.Attributes = Potion.Attributes.Stack(evt.Attributes);
                        evt.Accepted = true;
                    } else
                    {
                        Debug.LogError("Duplicate ingredient added to potion - inventory consumed incorrectly. FIXME");
                    }
                    // copy the attributes into the potion script being staged
                    // if there is a potion that
                    break;
                case CauldronEventType.PotionRemove:
                    // BUG? ignoring the evt.Attributes here because that could cause extra application of certain
                    // modifiers, such as potion value being set by ingredients, but a card setting it again
                    SpawnPotion();
                    break;
            }
        }
    }
}