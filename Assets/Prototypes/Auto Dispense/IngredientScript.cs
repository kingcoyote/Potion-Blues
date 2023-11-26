using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    public class IngredientScript : MonoBehaviour
    {
        private bool _selected;

        // Start is called before the first frame update
        void Start()
        {
            var selectHandler = gameObject.GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(Select);
            selectHandler.OnRelease.AddListener(Release);
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

        void Select()
        {
            _selected = true;
        }

        void Release()
        {
            _selected = false;
        }
    }
}