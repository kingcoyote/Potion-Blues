using PotionBlues.Definitions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class PotionScript : MonoBehaviour
    {
        public PotionDefinition Definition;
        public List<ShopAttributeValue> Attributes = new();
        public List<IngredientDefinition> Ingredients = new();

        public PotionState State = PotionState.Mixing;

        private float _brewTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            // TODO mark state as being prepared
            State = PotionState.Mixing;
            name = "New Potion";
        }

        // Update is called once per frame
        void Update()
        {
            // if state is brewing, reduce brew time
            // if brewing complete, mark state as done
        }

        public void StartBrewing()
        {
            _brewTime = Attributes.TryGet("Brewing Speed");
            Debug.Log($"Setting brewing speed to {_brewTime}");
        }

        public bool AddIngredient(IngredientDefinition ingredient)
        {
            // only allowed to add ingredients during mixing or mixed phases
            if (new[] { PotionState.Mixing, PotionState.Mixed }.Contains(State) == false)
                return false;

            // anything with less than 4 ingredients is some oddity
            if (ingredient.Attributes.Count < 4) 
                return false;

            // only allowed to add each ingredient a single time
            if (Ingredients.Contains(ingredient)) 
                return false;

            // no more primary ingredients allowed after it is mixed
            if (State == PotionState.Mixed && ingredient.Type == IngredientDefinition.IngredientType.Primary) 
                return false;

            Ingredients.Add(ingredient);
            Definition = PotionBlues.I().GetPotionType(Ingredients);
            if (Definition != null)
            {
                State = PotionState.Mixed;
                var secondaryCount = Ingredients.Where(i => i.Type == IngredientDefinition.IngredientType.Secondary).Count();
                name = $"Potion ({Definition.name}{string.Concat(Enumerable.Repeat('+', secondaryCount))})";
            }

            // TODO - if the ingredients now define a potion, mark state as ready to brew

            return true;
        }

        public enum PotionState
        {
            Mixing,
            Mixed,
            Brewing,
            Ready,
            Sold
        }
    }
}
