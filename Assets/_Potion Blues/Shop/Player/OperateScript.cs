using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VectorSwizzling;

namespace PotionBlues.Shop
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class OperateScript : MonoBehaviour
    {
        public bool IsComplete => _operatingTime >= _requiredOperatingTime;
        public Action OnComplete = () => { };

        private float _requiredOperatingTime;
        private float _fillSpeed = 1;
        private float _decaySpeed = 1;

        private PlayerInput _input;
        private bool _isOperating;
        private float _operatingTime;

        [SerializeField] private CircleCollider2D _box;
        [SerializeField] private Image _progressBar;
        [SerializeField] private Image _icon;

        // Start is called before the first frame update
        void Start()
        {
            _input = GameObject.Find("Player").GetComponent<PlayerInput>();
            _input.actions["Select"].performed += OnSelect;
            _input.actions["Select"].canceled += OnCancel;

            _box = GetComponent<CircleCollider2D>();

            OnComplete += () => { gameObject.SetActive(false); };
        }

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.actions["Select"].performed -= OnSelect;
                _input.actions["Select"].canceled -= OnCancel;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (IsComplete)
            {
                return;
            }

            if (_isOperating)
            {
                _operatingTime += Time.deltaTime * _fillSpeed;
            } else
            {
                _operatingTime -= Time.deltaTime * _decaySpeed;
            }

            _operatingTime = Mathf.Clamp(_operatingTime, 0, _requiredOperatingTime);
            _progressBar.fillAmount = _operatingTime / _requiredOperatingTime;

            if (_operatingTime >= _requiredOperatingTime)
            {
                gameObject.SetActive(false);
                OnComplete.Invoke();
            }
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            var cursor = Camera.main.ScreenToWorldPoint(_input.actions["Cursor"].ReadValue<Vector2>()).xy();
            if (_box.bounds.Contains(cursor) == false) return;

            _isOperating = true;
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            _isOperating = false;
        }

        public void Set(Sprite icon, float operatingTime)
        {
            _icon.sprite = icon;
            _requiredOperatingTime = operatingTime;
            _operatingTime = 0;
            gameObject.SetActive(true);
        }

        [Button]
        public void Set(Sprite icon, float operatingTime, float fillSpeed, float decaySpeed)
        {
            _fillSpeed = fillSpeed;
            _decaySpeed = decaySpeed;
            Set(icon, operatingTime);
            
        }
    }
}