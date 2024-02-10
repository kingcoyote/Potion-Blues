using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity;
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

        public void ResetShopObjects(List<RunUpgradeCard> objects)
        {
            ClearShopObjects();

            _box = GetComponent<BoxCollider2D>();
            var selectedObjects = objects.Where(card => card.IsSelected);
            int i = 0;

            foreach (var card in selectedObjects) {
                var shopObject = Instantiate(Prefab, transform);
                shopObject.SetDefinition(((ShopObjectUpgradeCardDefinition)card.Card).ShopObject);
                var position = (_box.size.y / 2) - ((i + 1.0) / (selectedObjects.Count() + 1.0) * _box.size.y);
                shopObject.transform.localPosition = Vector3.up * (float)position;
                i++;
            }
        }

        public void ClearShopObjects()
        {
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
        }
    }
}
