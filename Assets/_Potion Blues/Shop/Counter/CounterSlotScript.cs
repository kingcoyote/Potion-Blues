using PotionBlues.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class CounterSlotScript : MonoBehaviour
    {
        public PotionDefinition PotionType;
        [SerializeField]
        public Queue<List<ShopAttributeValue>> Potions = new();

        [SerializeField] private SpriteRenderer _potion;

        // Update is called once per frame
        void Update()
        {
            if (PotionType == null)
            {
                return;
            }

            if (Potions.Count == 0)
            {
                PotionType = null;
                _potion.sprite = null;
                return;
            }

            if (_potion.sprite == null)
            {
                _potion.sprite = PotionType.Potion;
            }

            foreach (var potion in Potions)
            {
                var shelfLife = potion.TryGet("Shelf Life");
                shelfLife -= Time.deltaTime;
                potion.Set("Shelf Life", shelfLife);
            }

            if (Potions.Peek().TryGet("Shelf Life") < 0)
            {
                Debug.Log($"Potion {PotionType.name} spoiled, discarding");
                Potions.Dequeue();
            }
        }
    }
}
