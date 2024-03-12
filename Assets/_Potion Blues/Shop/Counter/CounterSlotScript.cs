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
        public Queue<PotionData> Potions = new();

        [SerializeField] private SpriteRenderer _potion;
        [SerializeField] private TextMeshProUGUI _quantity;

        // Update is called once per frame
        void Update()
        {
            if (PotionType == null)
            {
                _quantity.text = string.Empty;
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

            _quantity.text = $"{Potions.Count}";

            foreach (var potion in Potions)
            {
                var shelfLife = potion.Attributes.TryGet("Shelf Life");
                shelfLife -= Time.deltaTime;
                potion.Attributes.Set("Shelf Life", shelfLife);
            }

            if (Potions.Peek().Attributes.TryGet("Shelf Life") < 0)
            {
                Debug.Log($"Potion {PotionType.name} spoiled, discarding");
                Potions.Dequeue();
            }
        }
    }
}
