using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class PotionScript : MonoBehaviour
    {
        public Color Color;

        private bool _selected;
        private CircleCollider2D _circleCollider;

        // Use this for initialization
        void Start()
        {
            _circleCollider = GetComponent<CircleCollider2D>();

            var selectHandler = GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(Select);
            selectHandler.OnRelease.AddListener(Release);
            selectHandler.Select();
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
                var cauldron = results[0].gameObject.GetComponent<CounterScript>();
                if (cauldron != null)
                {
                    cauldron.AddPotion(this);
                }
            }
        }
    }
}