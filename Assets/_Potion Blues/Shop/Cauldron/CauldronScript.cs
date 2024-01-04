using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System;

namespace PotionBlues.Shop
{
    public class CauldronScript : ShopObjectScript
    {
        [OnValueChanged("LoadCauldron")]
        public CauldronDefinition Cauldron;

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
            if (definition.GetType() != typeof(CauldronDefinition))
            {
                throw new ArgumentException($"CauldronScript cannot load an object of type {definition.GetType()}");
            }
            
            Cauldron = (CauldronDefinition)definition;
            LoadCauldron();
        }

        public void LoadCauldron()
        {

        }
    }
}