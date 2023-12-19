using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    public class DispenserScript : MonoBehaviour
    {
        [OnValueChanged("Refresh", includeChildren: true)]
        public PotionAttributeDefinition Attribute;
        public IngredientScript Ingredient;

        // Start is called before the first frame update
        void Start()
        {
            Refresh();
            var selectHandler = GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(() => { SpawnIngredient(); });
        }

        void Refresh()
        {
            var icon = transform.Find("Icon").GetComponent<SpriteRenderer>();

            icon.color = Attribute.Color;
            icon.sprite = Attribute.Icon;
        }

        private void SpawnIngredient()
        {
            var ingredient = Instantiate(Ingredient);
            ingredient.transform.position = transform.position;
            ingredient.Attribute = Attribute;
            ingredient.Select();
        }
    }
}