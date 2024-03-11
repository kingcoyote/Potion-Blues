using PotionBlues.Definitions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class CounterSlotScript : MonoBehaviour
    {
        public PotionDefinition PotionType;
        public Queue<List<ShopAttributeValue>> Potions = new();

        [SerializeField] private SpriteRenderer _potion;

        // Update is called once per frame
        void Update()
        {
            if (Potions.Count == 0)
            {
                PotionType = null;
            }

            if (PotionType != null && _potion.sprite == null)
            {
                _potion.sprite = PotionType.Potion;
            }
        }
    }
}
