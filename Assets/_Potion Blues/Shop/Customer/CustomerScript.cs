using UnityEngine;
using System.Collections;
using PotionBlues.Definitions;
using System.Collections.Generic;
using UnityEngine.UI;
using PotionBlues.Events;

namespace PotionBlues.Shop
{
    public class CustomerScript : MonoBehaviour
    {
        public float WalkSpeed = 2;
        public PotionDefinition DesiredPotion;
        public bool NeedsPotion => _potion == null;
        public bool IsWaiting => Mathf.Abs(_walkSpeed) <= 0.01f;

        public List<ShopAttributeValue> Attributes;

        private float _walkSpeed;
        private PotionData _potion;

        [SerializeField] private Animator _anim;
        [SerializeField] private Image _icon;

        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponent<Animator>();
            _walkSpeed = WalkSpeed;

            var matProps = new MaterialPropertyBlock();
            var renderer = GetComponent<SpriteRenderer>();
            renderer.GetPropertyBlock(matProps);
            matProps.SetColor("_VestColor", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(matProps);

            _icon.sprite = DesiredPotion.Potion;
            // scale desired potion icon to be 64px high, while still maintaing correct aspect ratio
            _icon.GetComponent<RectTransform>().sizeDelta = DesiredPotion.Potion.textureRect.size * (64 / DesiredPotion.Potion.textureRect.size.y);

            Refresh();
        }

        // Update is called once per frame
        void Update()
        {
            _anim.SetBool("Walking", Mathf.Abs(_walkSpeed) > WalkSpeed * 0.9f);

            transform.position += Vector3.right * _walkSpeed * Time.deltaTime;

            var customerPatience = Attributes.TryGet("Customer Patience");

            if (Mathf.Abs(_walkSpeed) < 0.01f)
            {
                customerPatience -= Time.deltaTime;
            }

            if (customerPatience < 0 || _potion != null)
            {
                _walkSpeed = -WalkSpeed;
                var xdir = _walkSpeed >= 0 ? 1 : -1;
                transform.localScale = new Vector3(xdir, 1, 1);
            }

            Attributes.Set("Customer Patience", customerPatience);
        }

        void Refresh()
        {
            // _icon.sprite = Attribute.Icon;
            // _icon.color = Attribute.Color;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Queue")
            {
                StartCoroutine(StopWalking());
            }

            var isReadyToLeave = Attributes.TryGet("Customer Patience") <= 0.01f || _potion != null;

            if (other.gameObject.GetComponent<DoorScript>() != null && isReadyToLeave)
            {
                DoorEvent evt;

                if (_potion == null)
                {
                    evt = new DoorEvent(DoorEventType.CustomerLeave, Attributes);
                } else {
                    evt = new DoorEvent(DoorEventType.CustomerLeave, Attributes)
                    {
                        Potion = _potion
                    };
                }

                PotionBlues.I().EventBus.Raise(evt);
                Destroy(gameObject);
            }
        }

        private IEnumerator StopWalking()
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));

            for (int i = 0; i < 10; i++)
            {
                _walkSpeed -= WalkSpeed / 10;
                yield return new WaitForSeconds(0.1f);
            }

            _walkSpeed = 0;
        }

        public bool BuyPotion(PotionData potion)
        {
            if (potion.Potion != DesiredPotion)
            {
                return false;
            }

            _potion = potion;

            return true;
        }
    }
}