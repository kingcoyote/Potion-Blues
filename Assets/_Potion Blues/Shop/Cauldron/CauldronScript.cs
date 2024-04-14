using System.Collections.Generic;
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

        [FoldoutGroup("Prefab"), SerializeField] private PotionScript _potionPrefab;
        [FoldoutGroup("Prefab"), SerializeField] private BoxCollider2D _box;
        [FoldoutGroup("Prefab"), SerializeField] private SpriteRenderer _fill;
        [FoldoutGroup("Prefab"), SerializeField] private SpriteRenderer _sprite;
        [FoldoutGroup("Prefab/Operator"), SerializeField] private OperateScript _operate;
        [FoldoutGroup("Prefab/Operator"), SerializeField] private Sprite _brewingIcon;
        [FoldoutGroup("Prefab/Operator"), SerializeField] private Sprite _cleaningIcon;
        [FoldoutGroup("Prefab/Effects"), SerializeField] private ParticleSystem _brewing;
        [FoldoutGroup("Prefab/Effects"), SerializeField] private ParticleSystem _cleaning;

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
        }

        public void OnDestroy()
        {
            if (_bus != null)
            {
                _bus.UnsubscribeFromTarget<CauldronEvent>(this, OnCauldronEvent);
            }

            if (_input != null)
            {
                _input.actions["Select"].performed -= OnSelect;
            }
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
                        if (Potion.State == PotionScript.PotionState.Mixed)
                        {
                            State = CauldronState.Ready;
                            _operate.Set(_brewingIcon, Attributes.TryGet("Brewing Time"));
                            _operate.OnComplete = OnBrewing;
                        } else
                        {
                            State = CauldronState.Empty;
                        }
                        evt.Accepted = true;
                    }
                    else
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

        private void SpawnPotion()
        {
            Potion.Select();
            Potion.Show();
            Potion = null;

            if (RemainingPotions <= 0)
            {
                // TODO - set Operator to the clean cauldron icon and set cleaning time
                _operate.Set(_cleaningIcon, Attributes.TryGet("Cleaning Time"));
                _operate.OnComplete = OnClean;
                _sprite.sprite = Cauldron.Dirty;
                State = CauldronState.Dirty;
                return;
            }
        }

        private void OnSelect(InputAction.CallbackContext _context)
        {
            if (State != CauldronState.Ready) return;
            if (Potion == null || Potion.State != PotionScript.PotionState.Ready) return;

            var cursor = Camera.main.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            if (_box.bounds.Contains(cursor) == false) return;

            _bus.Raise(new CauldronEvent(CauldronEventType.PotionRemove, Potion.Attributes), this, this);
        }

        private void StartClean()
        {
            var cleaningTime = Attributes.TryGet("Cleaning Time");
            var bubbleProperties = _cleaning.main;
            bubbleProperties.duration = cleaningTime;
            _cleaning.Play();
            State = CauldronState.Cleaning;
        }

        private void OnClean() { 
            State = CauldronState.Ready;
            _sprite.sprite = Cauldron.Sprite;
            RemainingPotions = (int)Attributes.TryGet("Cleaning Interval");
        }

        private void StartBrewing()
        {
            if (Potion.State != PotionScript.PotionState.Mixed) return;
            Potion.Attributes = Potion.Attributes.Stack(Attributes);
            Potion.State = PotionScript.PotionState.Brewing;
            BrewTime = Attributes.TryGet("Brewing Time");


            var bubbleProperties = _brewing.main;
            bubbleProperties.duration = BrewTime;
            _brewing.Play();
        }

        private void OnBrewing()
        {
            Potion.Attributes = Potion.Attributes.Stack(Attributes);
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