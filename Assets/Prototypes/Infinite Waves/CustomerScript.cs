using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.InfiniteWaves
{
    public class CustomerScript : MonoBehaviour
    {
        public float Speed;

        [OnValueChanged("UpdateColor")]
        public Color Color;

        private void Start()
        {
            UpdateColor();
        }

        // Start is called before the first frame update
        void UpdateColor()
        {
            GetComponent<SpriteRenderer>().color = Color;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = transform.position + Vector3.left * Speed * Time.deltaTime;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Counter")
            {
                Speed = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var potion = collision.GetComponent<PotionScript>();
            if (potion != null)
            {
                Destroy(potion.gameObject);
                Destroy(gameObject);
            }
        }
    }
}