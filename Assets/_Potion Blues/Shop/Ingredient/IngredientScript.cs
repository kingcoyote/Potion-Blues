using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System;

namespace PotionBlues.Shop
{
    public class IngredientScript : ShopObjectScript
    {
        [OnValueChanged("LoadIngredient")]
        public IngredientDefinition Ingredient;

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
            if (definition.GetType() != typeof(IngredientDefinition))
            {
                throw new ArgumentException($"IngredientScript cannot load an object of type {definition.GetType()}");
            }

            Ingredient = (IngredientDefinition)definition;
            LoadIngredient();
        }

        public void LoadIngredient()
        {

        }
    }
}