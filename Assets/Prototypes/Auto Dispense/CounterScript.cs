using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CounterScript : MonoBehaviour
    {
        public int PotionCapacity;
        public List<PotionScript> Potions;
        private BoxCollider2D _box;

        private void Start()
        {
            _box = GetComponent<BoxCollider2D>();
        }

        public void AddPotion(PotionScript potion)
        {
            if (Potions.Count >= PotionCapacity) return;
            
            Potions.Add(potion);
            potion.transform.SetParent(transform);
            potion.transform.localPosition = new Vector3(0, Random.Range(-_box.size.y, _box.size.y)/3, 0);
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            var customer = collision.gameObject.GetComponent<CustomerScript>();
            if (customer != null) TryDispense(customer);
        }

        private void TryDispense(CustomerScript customer)
        {
            if (Potions.Count <= 0) return;
            customer.BuyPotion(Potions[0]);
            Potions.RemoveAt(0);
            // Destroy(customer.Potion.gameObject);
        }
    }
}