using UnityEngine;
using PotionBlues.Definitions;
using UnityEngine.InputSystem;
using VectorSwizzling;

namespace PotionBlues.Shop
{
    public class IngredientScript : MonoBehaviour
    {
        public IngredientDefinition Ingredient;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private BoxCollider2D _box;
        private PlayerInput _player;
        private bool _isSelected;

        // Use this for initialization
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<PlayerInput>();
            _player.actions["Select"].performed += OnSelect;
            _player.actions["Select"].canceled += OnDeselect;
            _isSelected = true;
            _sprite.sprite = Ingredient.Ingredient;
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            var cursor = _player.camera.ScreenToWorldPoint(_player.actions["Cursor"].ReadValue<Vector2>()).xy();
            if (_box.bounds.Contains(cursor))
            {
                _isSelected = true;
            }
        }

        void OnDeselect(InputAction.CallbackContext _context)
        {
            _isSelected = false;
        }    

        // Update is called once per frame
        void Update()
        {
            if (!_isSelected) return;
            var cursor = _player.camera.ScreenToWorldPoint(_player.actions["Cursor"].ReadValue<Vector2>()).xy();
            transform.position = cursor;
        }
    }
}