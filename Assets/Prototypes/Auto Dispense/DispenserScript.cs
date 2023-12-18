using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    public class DispenserScript : MonoBehaviour
    {
        public IngredientScript Ingredient;
        [OnValueChanged("Refresh")]
        public Color Color;

        // Start is called before the first frame update
        void Start()
        {
            Refresh();
            var selectHandler = GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(() => { SpawnIngredient(); });
        }

        void Refresh()
        {
            GetComponent<SpriteRenderer>().color = Color;
        }

        private void SpawnIngredient()
        {
            var ingredient = Instantiate(Ingredient);
            ingredient.transform.position = transform.position;
            ingredient.Select();
        }
    }
}