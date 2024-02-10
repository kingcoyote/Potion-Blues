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
        [BoxGroup("Instance")] public float CustomerFrequency;
        [BoxGroup("Instance")] public float CustomerPatience;
        [BoxGroup("Instance")] public float CustomerTipping;
        [BoxGroup("Instance")] public float ReputationBonus;

        [BoxGroup("Customers")] public CustomerScript CustomerPrefab;
        [BoxGroup("Customers")] public BoxCollider2D DoorBox;

        private bool _shopOpen;

        // Use this for initialization
        new public void Start()
        {
            base.Start();

            _bus.SubscribeToTarget<DoorEvent>(this, OnDoorEvent);
            _bus.Raise(new DoorEvent(DoorEventType.Spawn, Definition.Attributes), this, this);

            _bus.SubscribeTo<RunEvent>(OnRunEvent);

            _shopOpen = true;
            StartCoroutine(SpawnCustomers());
        }

        private IEnumerator SpawnCustomers()
        {
            while (_shopOpen)
            {
                SpawnCustomer();
                yield return new WaitForSeconds(1 / CustomerFrequency);
            }
            Debug.Log("Shop closed");
        }

        private void SpawnCustomer()
        {
            Debug.Log("Spawning customer");
            var customer = Instantiate(CustomerPrefab, transform);
            var offsety = (PotionBlues.I().RNG.NextFloat() - 0.5f) * DoorBox.size.y / 2;
            customer.transform.position += new Vector2(DoorBox.offset.x, offsety).xy0();
            customer.CustomerPatience = CustomerPatience;
            customer.CustomerTipping = CustomerTipping;
            customer.ReputationBonus = ReputationBonus;
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

        }

        void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {

            switch (evt.Type)
            {
                case DoorEventType.Spawn:
                    CustomerFrequency = evt.Attributes.Find(a => a.Attribute.name == "Customer Frequency").Value;
                    CustomerPatience = evt.Attributes.Find(a => a.Attribute.name == "Customer Patience").Value;
                    CustomerTipping = evt.Attributes.Find(a => a.Attribute.name == "Customer Tipping").Value;
                    ReputationBonus = evt.Attributes.Find(a => a.Attribute.name == "Reputation Bonus").Value;
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
    }
}