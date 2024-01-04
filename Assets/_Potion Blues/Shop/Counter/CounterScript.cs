using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System;

namespace PotionBlues.Shop
{
    public class CounterScript : ShopObjectScript
    {
        [OnValueChanged("LoadCounter")]
        public CounterDefinition Counter;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void LoadShopObject(ShopObjectDefinition definition)
        {
            if (definition.GetType() != typeof(CounterDefinition))
            {
                throw new ArgumentException($"CounterScript cannot load an object of type {definition.GetType()}");
            }

            Counter = (CounterDefinition)definition;
            LoadCounter();
        }

        public void LoadCounter()
        {

        }
    }
}