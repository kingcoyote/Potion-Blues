using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using VectorSwizzling;
using PotionBlues.Events;
using PotionBlues.Definitions;

namespace PotionBlues.Shop
{
    public class CauldronScript : ShopObjectScript
    {
        public CauldronDefinition Cauldron => (CauldronDefinition)Definition;

        [BoxGroup("Instance")] public List<ShopAttributeValue> Attributes;

        [BoxGroup("State")] public PotionScript Potion;
        [BoxGroup("State")] public int RemainingPotions;
        [BoxGroup("State")] public float BrewTime;
        [BoxGroup("State")] public CauldronState State;

        [BoxGroup("Prefab"), SerializeField] private PotionScript _potionPrefab;
        [BoxGroup("Prefab"), SerializeField] private BoxCollider2D _box;
        [BoxGroup("Prefab"), SerializeField] private SpriteRenderer _fill;
        [BoxGroup("Prefab"), SerializeField] private SpriteRenderer _sprite;
        [BoxGroup("Prefab/Effects"), SerializeField] private ParticleSystem _brewing;
        [BoxGroup("Prefab/Effects"), SerializeField] private ParticleSystem _cleaning;

        private PlayerInput _input;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CauldronEvent>(this, OnCauldronEvent);
            _bus.Raise(new CauldronEvent(CauldronEventType.Spawn, Definition.Attributes), this, this);

            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].performed += OnSelect;

            var fillOffset = (_sprite.sprite.textureRect.height / _sprite.sprite.pixelsPerUnit) / 2;
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
            if (_box.bounds.Contains(cursor) == false) return;
            
            switch (State)
            {
                case CauldronState.Dirty:
                    StartCoroutine(CleanCauldron());
                    break;
                case CauldronState.Ready:
                    if (Potion == null) return;
                    switch (Potion.State)
                    {
                        case PotionScript.PotionState.Mixed:
                            StartBrewing();
                            break;
                        case PotionScript.PotionState.Ready:
                            _bus.Raise(new CauldronEvent(CauldronEventType.PotionRemove, Potion.Attributes), this, this);
                            break;
                    }
                    break;
            }
        }

        private IEnumerator CleanCauldron()
        {
            var cleaningTime = Attributes.TryGet("Cleaning Time");
            var bubbleProperties = _cleaning.main;
            bubbleProperties.duration = cleaningTime;
            _cleaning.Play();
            State = CauldronState.Cleaning;
            yield return new WaitForSeconds(cleaningTime);
            State = CauldronState.Ready;
            _sprite.sprite = Cauldron.Sprite;
            RemainingPotions = (int)Attributes.TryGet("Cleaning Interval");
        }

        private void SpawnPotion()
        {
            Debug.Log("Spawning potions");
            Potion.Select();
            Potion.Show();
            Potion = null;

            if (RemainingPotions <= 0)
            {
                _sprite.sprite = Cauldron.Dirty;
                State = CauldronState.Dirty;
                return;
            }
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
            var bubbleProperties = _brewing.main;
            bubbleProperties.duration = BrewTime;
            // bubbleProperties.startColor = Potion.Definition.Color;
            _brewing.Play();
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
                Potion = Instantiate(_potionPrefab, transform);
                _bus.Raise(new CauldronEvent(CauldronEventType.BrewFailed, Potion.Attributes));

                if (RemainingPotions <= 0)
                {
                    _sprite.sprite = Cauldron.Dirty;
                    State = CauldronState.Dirty;
                    return;
                }
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
                        Potion = Instantiate(_potionPrefab, transform);
                        Potion.Cauldron = this;
                    }

                    if (Potion.AddIngredient(evt.Ingredient))
                    {
                        Potion.Attributes = Potion.Attributes.Stack(evt.Attributes);
                        State = Potion.State == PotionScript.PotionState.Mixed ? CauldronState.Ready : CauldronState.Empty;
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

    public enum CauldronState
    {
        Empty,
        Ready,
        Brewing,
        Dirty,
        Cleaning
    }
}