using Sirenix.OdinInspector;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    public class IngredientScript : MonoBehaviour
    {
        [OnValueChanged("Refresh")]
        public PotionAttributeDefinition Attribute;

        private bool _selected;
        private CircleCollider2D _circleCollider;

        // Start is called before the first frame update
        void Start()
        {
            var selectHandler = gameObject.GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(Select);
            selectHandler.OnRelease.AddListener(Release);

            _circleCollider = GetComponent<CircleCollider2D>();

            Refresh();
        }

        void Refresh()
        {
            var sprite = GetComponent<SpriteRenderer>();
            sprite.sprite = Attribute.Ingredient;
        }

        // Update is called once per frame
        void Update()
        {
            if (_selected)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            }
        }

        public void Select()
        {
            _selected = true;
        }

        public void Release()
        {
            _selected = false;

            Collider2D[] results = new Collider2D[4];
            var filter = new ContactFilter2D();
            filter.NoFilter();
            if (_circleCollider.OverlapCollider(filter, results) > 0)
            {
                var cauldron = results[0].gameObject.GetComponent<CauldronScript>();
                if (cauldron != null)
                {
                    cauldron.AddIngredient(this);
                }
            }
        }
    }
}