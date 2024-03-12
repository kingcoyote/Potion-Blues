using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorSwizzling;

namespace PotionBlues.Shop
{
    public class DoorScript : ShopObjectScript
    {
        [BoxGroup("Instance")] public List<ShopAttributeValue> Attributes;

        [BoxGroup("Customers")] public CustomerScript CustomerPrefab;
        [BoxGroup("Customers")] public BoxCollider2D DoorBox;

        private bool _shopOpen;
        private float _nextCustomer;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<DoorEvent>(this, OnDoorEvent);
            _bus.Raise(new DoorEvent(DoorEventType.Spawn, Definition.Attributes), this, this);

            _bus.SubscribeTo<RunEvent>(OnRunEvent);

            _shopOpen = true;
        }

        public void OnDestroy()
        {
            if (_bus != null)
            {
                _bus.UnsubscribeFromTarget<DoorEvent>(this, OnDoorEvent);
                _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_shopOpen) return;
            _nextCustomer -= Time.deltaTime;
            if (_nextCustomer <= 0)
            {
                _bus.Raise(new DoorEvent(DoorEventType.CustomerArrive, Definition.Attributes), this, this);
            }
        }

        void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {
            Attributes = evt.Attributes;

            switch (evt.Type)
            {
                case DoorEventType.Spawn:
                    _nextCustomer = 3 * (1 / Attributes.TryGet("Customer Frequency"));
                    break;
                case DoorEventType.CustomerArrive:
                    SpawnCustomer();
                    _nextCustomer = 1 / Attributes.TryGet("Customer Frequency");
                    break;
                case DoorEventType.CustomerLeave:
                    DespawnCustomer();
                    break;

            }
        }

        void OnRunEvent(ref RunEvent evt)
        {
            switch (evt.Type)
            {
                case RunEventType.ShopClosed:
                    _shopOpen = false;
                    break;
            }
        }

        private void SpawnCustomer()
        {
            // Debug.Log("Spawning customer");
            var customer = Instantiate(CustomerPrefab, transform);
            var offsety = (PotionBlues.I().RNG.NextFloat() - 0.5f) * DoorBox.size.y / 2;
            customer.transform.position += new Vector2(DoorBox.offset.x, offsety).xy0();
            customer.Attributes = Attributes;
            customer.DesiredPotion = PotionBlues.I().GameData.ActiveRun.GetRandomPotion();
        }

        private void DespawnCustomer()
        {

        }
    }
}