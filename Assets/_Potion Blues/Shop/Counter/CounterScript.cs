using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;

using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class CounterScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public float ShelfLife;
        [BoxGroup("Instance")] public int CounterSlots;
        [BoxGroup("Instance")] public int SlotCapacity;

        // TODO - wtf
        // events can't store potionscript because its in the wrong namespace
        // but definitions don't store details of a specific potion, including
        // the applied attributes and spoil time
        public Dictionary<PotionDefinition, Queue<List<ShopAttributeValue>>> Potions;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private BoxCollider2D _box;

        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<CounterEvent>(this, OnCounterEvent);
            _bus.Raise(new CounterEvent(CounterEventType.Spawn, Definition.Attributes), this, this);

            _box.size = new Vector2(_sprite.sprite.textureRect.width, _sprite.sprite.textureRect.height) / _sprite.sprite.pixelsPerUnit;
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
                    ShelfLife = evt.Attributes.Find(a => a.Attribute.name == "Shelf Life").Value;
                    SlotCapacity = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Capacity").Value;
                    CounterSlots = (int)evt.Attributes.Find(a => a.Attribute.name == "Counter Slots").Value;
                    Potions = new Dictionary<PotionDefinition, Queue<List<ShopAttributeValue>>>(CounterSlots);
                    break;
                case CounterEventType.PotionAdd:
                    if (Potions.ContainsKey(evt.Potion)) {
                        if (Potions[evt.Potion].Count < SlotCapacity)
                        {
                            // BUG - need to add it more than once if brewing output > 1
                            Potions[evt.Potion].Enqueue(evt.Attributes);
                            Debug.Log($"Adding {evt.Potion.name} to existing queue");
                        }
                    } else if (Potions.Count < CounterSlots)
                    {
                        Potions.Add(evt.Potion, new Queue<List<ShopAttributeValue>>(SlotCapacity));
                        Potions[evt.Potion].Enqueue(evt.Attributes);
                        Debug.Log($"Creating new queue for {evt.Potion.name}");

                    } else
                    {
                        Debug.LogError("Unable to store excess potion, deleting it. FIXME");
                    }
                    break;
            }
        }
    }
}