using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    public class DoorScript : MonoBehaviour
    {
        public float SpawnTime;
        public float SpawnDecay;
        public CustomerScript Customer;

        [MinMaxSlider(0, 30)]
        public Vector2 PatienceRange;

        private float _spawnCooldown;
        private BoxCollider2D _box;

        // Start is called before the first frame update
        void Start()
        {
            _box = GetComponent<BoxCollider2D>();
            _spawnCooldown = SpawnTime;
            SpawnCustomer();
        }

        // Update is called once per frame
        void Update()
        {
            _spawnCooldown -= Time.deltaTime;
            if (_spawnCooldown < 0)
            {
                _spawnCooldown = SpawnTime;
                SpawnTime *= SpawnDecay;
                SpawnCustomer();
            }
        }

        void SpawnCustomer()
        {
            var customer = Instantiate(Customer, transform);
            customer.transform.position = transform.position 
                + Vector3.down * Random.Range(-_box.size.y, _box.size.y) / 2
                + Vector3.right * _box.size.x;
            customer.Patience = Random.Range(PatienceRange.x, PatienceRange.y);
        }
    }
}
