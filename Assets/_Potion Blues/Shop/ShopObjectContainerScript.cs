using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace PotionBlues.Shop
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShopObjectContainerScript : MonoBehaviour
    {
        [ValueDropdown("@ShopObjectCategoryDefinition.GetCategories()")]
        public ShopObjectCategoryDefinition Category;
        public ShopObjectScript Prefab;

        private BoxCollider2D _box;

        // Start is called before the first frame update
        void Start()
        {
            _box = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetShopObjects(List<ShopObjectDefinition> objects)
        {
            Debug.Log($"{name} is resetting shop objects");

            if (Application.isPlaying)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            else
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }

            _box = GetComponent<BoxCollider2D>();

            var n = objects.Count();
            for (int i = 0; i < n; i++)
            {
                var shopObject = Instantiate(Prefab, transform);
                shopObject.LoadDefinition(objects[i]);
                shopObject.name = objects[i].name;
                var position = (_box.size.y / 2) - ((i + 1.0) / (n + 1.0) * _box.size.y);
                shopObject.transform.localPosition = Vector3.up * (float)position;
            }
        }
    }
}
