using PotionBlues.Definitions;
using PotionBlues.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VectorSwizzling;

namespace PotionBlues.Shop
{
    public class PotionScript : MonoBehaviour
    {
        public PotionDefinition Definition;
        public List<ShopAttributeValue> Attributes = new();
        public List<IngredientDefinition> Ingredients = new();
        public CauldronScript Cauldron;
        public PotionState State = PotionState.Mixing;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private BoxCollider2D _box;
        private PlayerInput _input;
        private bool _isSelected;

        // Start is called before the first frame update
        void Start()
        {
            // TODO mark state as being prepared
            State = PotionState.Mixing;
            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].canceled += OnDeselect;
            name = "New Potion";

            _sprite.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isSelected) return;
            var cursor = _input.camera.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            transform.position = cursor;
        }

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.actions["Select"].canceled -= OnDeselect;
        }
        }

        public bool AddIngredient(IngredientDefinition ingredient)
        {
            // only allowed to add ingredients during mixing or mixed phases
            if (new[] { PotionState.Mixing, PotionState.Mixed }.Contains(State) == false)
                return false;

            // anything with less than 4 ingredients is some oddity
            if (ingredient.Attributes.Count < 4) 
                return false;

            // only allowed to add each ingredient a single time
            if (Ingredients.Contains(ingredient)) 
                return false;

            // no more primary ingredients allowed after it is mixed
            if (State == PotionState.Mixed && ingredient.Type == IngredientDefinition.IngredientType.Primary) 
                return false;

            Ingredients.Add(ingredient);
            Definition = PotionBlues.I().GetPotionType(Ingredients);
            if (Definition != null)
            {
                State = PotionState.Mixed;
                _sprite.sprite = Definition.Potion;

                var secondaryCount = Ingredients.Where(i => i.Type == IngredientDefinition.IngredientType.Secondary).Count();
                name = $"Potion ({Definition.name}{string.Concat(Enumerable.Repeat('+', secondaryCount))})";
            }

            return true;
        }

        public void Show()
        {
            _sprite.enabled = true;
        }

        public void Hide()
        {
            _sprite.enabled = false;
        }

        public void Select()
        {
            _isSelected = true;
        }

        void OnDeselect(InputAction.CallbackContext _context)
        {
            if (_isSelected == false) return;

            Debug.Log("Deselecting potion");

            _isSelected = false;

            // check if the ingredient is currently overlapping a shop object
            var filter = new ContactFilter2D();
            filter.SetLayerMask(1 << LayerMask.NameToLayer("Shop Object"));
            Collider2D[] results = new Collider2D[1];
            if (_box.OverlapCollider(filter, results) == 0)
            {
                Debug.Log("Dropping on nothing, returning to cauldron");
                StartCoroutine(ReturnToCauldron());
                return;
            }

            var counter = results[0].GetComponent<CounterScript>();
            if (counter == null)
            {
                Debug.Log("dropped on something, but not a counter. returning to cauldron");
                StartCoroutine(ReturnToCauldron());
                return;
            }

            Debug.Log($"Dropping potion into counter {counter.name}");
            // TODO signal to cauldron that it can create a new potion or require cleaning
            // what happens if the counter rejects the potion?
            PotionBlues.I().EventBus.Raise(
                new CounterEvent(CounterEventType.PotionAdd, Attributes)
                {
                    Potion = Definition
                },
                counter,
                Cauldron
            );

            Destroy(gameObject);
        }

        private IEnumerator ReturnToCauldron()
        {
            var startTime = Time.fixedTime;
            var endTime = startTime + 0.25f;
            var startPos = transform.position;
            var endPos = Cauldron.transform.position;
            while (endTime > Time.fixedTime)
            {
                yield return null;
                transform.position = Vector2.Lerp(startPos, endPos, Mathf.InverseLerp(startTime, endTime, Time.fixedTime));
            }
            Hide();
        }

        public enum PotionState
        {
            Mixing,
            Mixed,
            Brewing,
            Ready
        }
    }
}
