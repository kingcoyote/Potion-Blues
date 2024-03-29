﻿using UnityEngine;
using PotionBlues.Definitions;
using UnityEngine.InputSystem;
using VectorSwizzling;
using System.Collections;
using PotionBlues.Events;
using System.Collections.Generic;

namespace PotionBlues.Shop
{
    public class IngredientScript : MonoBehaviour
    {
        public IngredientDefinition Ingredient;
        public IngredientBinScript IngredientBin;
        public List<ShopAttributeValue> Attributes;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private BoxCollider2D _box;
        private PlayerInput _input;
        private bool _isSelected;

        // Use this for initialization
        void Start()
        {
            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].canceled += OnDeselect;
            _isSelected = true;
            _sprite.sprite = Ingredient.Ingredient;
        }

        void OnDeselect(InputAction.CallbackContext _context)
        {
            if (_isSelected == false) return;

            _isSelected = false;
            // removed OnDeselect because either this ingredient is returning to the bin, or being added to a cauldron
            _input.actions["Select"].canceled -= OnDeselect;

            // check if the ingredient is currently overlapping a shop object
            var filter = new ContactFilter2D();
            filter.SetLayerMask(1 << LayerMask.NameToLayer("Shop Object"));
            Collider2D[] results = new Collider2D[1];
            if (_box.OverlapCollider(filter, results) == 0)
            {
                StartCoroutine(ReturnToBin());
                return;
            }

            var cauldron = results[0].GetComponent<CauldronScript>();
            if (cauldron == null)
            {
                StartCoroutine(ReturnToBin());
                return;
            }
            
            // what happens if the cauldron rejects the ingredient?
            PotionBlues.I().EventBus.Raise(
                new CauldronEvent(CauldronEventType.IngredientAdd, Attributes)
                {
                    Ingredient = Ingredient
                },
                cauldron,
                IngredientBin
            );
            Destroy(gameObject);
        }    

        public IEnumerator ReturnToBin()
        {
            var startTime = Time.fixedTime;
            var endTime = startTime + 0.25f;
            var startPos = transform.position;
            var endPos = IngredientBin.transform.position;
            while (endTime > Time.fixedTime)
            {
                yield return null;
                transform.position = Vector2.Lerp(startPos, endPos, Mathf.InverseLerp(startTime, endTime, Time.fixedTime));
            }
            IngredientBin.Quantity += 1;
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isSelected) return;
            var cursor = _input.camera.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            transform.position = cursor;
        }
    }
}