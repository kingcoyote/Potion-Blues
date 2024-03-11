using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class CounterScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public float ShelfLife;
        [BoxGroup("Instance")] public int CounterSlots;
        [BoxGroup("Instance")] public int SlotCapacity;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private BoxCollider2D _box;

        private List<CounterSlotScript> _slots = new();
        [SerializeField] private CounterSlotScript _slotPrefab;

        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CounterEvent>(this, OnCounterEvent);
            _bus.Raise(new CounterEvent(CounterEventType.Spawn, Definition.Attributes), this, this);
        }

        public void OnDestroy()
        {
            if (_bus != null)
                _bus.UnsubscribeFromTarget<CounterEvent>(this, OnCounterEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCounterEvent(ref CounterEvent evt, IEventNode target, IEventNode source)
        {
            switch (evt.Type)
            {
                case CounterEventType.Spawn:
                    SpawnCounter(ref evt);
                    break;
                case CounterEventType.PotionAdd:
                    AddPotion(ref evt);
                    break;
            }
        }

        private void SpawnCounter(ref CounterEvent evt)
        {
            _box.size = new Vector2(_sprite.sprite.textureRect.width, _sprite.sprite.textureRect.height) / _sprite.sprite.pixelsPerUnit;

            ShelfLife = evt.Attributes.Find(a => a.Attribute.name == "Shelf Life").Value;
            SlotCapacity = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Capacity").Value;
            CounterSlots = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Slots").Value;

            foreach (var slot in _slots)
            {
                Destroy(slot.gameObject);
            }

            _slots = new List<CounterSlotScript>();
            for (int i = 0; i < CounterSlots; i++)
            {
                var newSlot = Instantiate(_slotPrefab, transform);
                // evenly distribute counter slots from -1 to 1, such as:
                // 3 = -0.5, 0, 0.5 (-2/4, 0, 2/4)
                // 4 = -0.6, 0.2, 0.2, 0.6 (-3/5, -1/5, 1/5, 3/5)
                // 5 = -0.66, -0.33, 0, 0.33, 0.66 (-4/6, -2/6, 0, 2/6, 4/6)
                // 6 = -5/7, -3/7, -1/7, 1/7, 3/7, 5/7
                var counterSize = _box.size.y / 2.0f * (CounterSlots - 1.0f) / (CounterSlots + 1.0f);
                var boxOffset = Vector3.up * counterSize;
                newSlot.transform.localPosition = Vector3.Lerp(
                    boxOffset,
                    -boxOffset,
                    i / (CounterSlots - 1f)
                );
                newSlot.name = $"Counter Slot #{i + 1}";
                _slots.Add(newSlot);
            }
        }

        private void AddPotion(ref CounterEvent evt)
        {
            var potionType = evt.Potion;
            var existingSlots = _slots.Where(slot => slot.PotionType == potionType);
            var openSlots = _slots.Where(slot => slot.PotionType == null);

            if (existingSlots.Count() > 0)
            {
                var slot = existingSlots.First();
                if (slot.Potions.Count < SlotCapacity)
                {
                    // BUG - need to add it more than once if brewing output > 1
                    slot.Potions.Enqueue(evt.Attributes);
                    Debug.Log($"Adding {evt.Potion.name} to existing queue");
                } else
                {
                    Debug.LogError("Unable to store potion that is beyond slot capacity, deleting it. FIXME");
                }
            }
            else if (openSlots.Count() > 0)
            {
                var slot = openSlots.First();
                slot.PotionType = evt.Potion;
                slot.Potions.Enqueue(evt.Attributes);
                Debug.Log($"Creating new queue for {evt.Potion.name}");
            }
            else
            {
                Debug.LogError("Unable to store potion that doesn't have a free slot, deleting it. FIXME");
            }
        }
    }
}