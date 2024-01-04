using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System;

namespace PotionBlues.Shop
{
    public class DoorScript : ShopObjectScript
    {
        [OnValueChanged("LoadDoor")]
        public DoorDefinition Door;

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
            if (definition.GetType() != typeof(DoorDefinition))
            {
                throw new ArgumentException($"DoorScript cannot load an object of type {definition.GetType()}");
            }

            Door = (DoorDefinition)definition;
            LoadDoor();
        }

        public void LoadDoor()
        {

        }
    }
}