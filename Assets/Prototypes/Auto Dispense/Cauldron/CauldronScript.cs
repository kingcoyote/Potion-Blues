using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    [RequireComponent(typeof(SelectHandlerScript))]
    public class CauldronScript : MonoBehaviour
    {
        public float BrewTime;

        [SerializeField] private CounterScript _counter;
        [SerializeField] private SpriteRenderer _cauldronFill;
        [SerializeField] private SpriteRenderer _cauldronFillLeft;
        [SerializeField] private SpriteRenderer _cauldronFillRight;
        [SerializeField] private Sprite _potEmpty;
        [SerializeField] private Sprite _potHalf;
        [SerializeField] private Sprite _potFull;
        [SerializeField] private PotionScript _potionPrefab;

        private List<IngredientScript> _ingredients = new();
        private float _brewTime;
        private Animator _anim;

        // Start is called before the first frame update
        void Start()
        {
            _cauldronFill.color = Color.clear;
            _cauldronFillLeft.color = Color.clear;
            _cauldronFillRight.color = Color.clear;

            _anim = GetComponent<Animator>();

            var selectHandler = GetComponent<SelectHandlerScript>();
            selectHandler.OnSelect.AddListener(SpawnPotion);
        }

        public void Update()
        {
            _anim.SetFloat("Brewing", Mathf.Clamp01(_brewTime / BrewTime));
        }

        public void AddIngredient(IngredientScript ingredient)
        {
            switch (_ingredients.Count)
            {
                case 0:
                    _ingredients.Add(ingredient);
                    Destroy(ingredient.gameObject);
                    _cauldronFill.sprite = _potHalf;
                    _cauldronFill.color = ingredient.Attribute.Color;
                    break;
                case 1:
                    _ingredients.Add(ingredient);
                    Destroy(ingredient.gameObject);
                    StartBrewing();
                    break;
                default:
                case 2:
                    break;
            }
        }

        private void SpawnPotion()
        {
            if (_brewTime < BrewTime)
            {
                return;
            }

            _cauldronFill.color = Color.clear;
            var potion = Instantiate(_potionPrefab);
            potion.Attribute = _ingredients[0].Attribute;
            potion.transform.position = transform.position;
            potion.Select();
            
            _brewTime = 0;
            _ingredients.Clear();
        }

        private void StartBrewing()
        {
            StartCoroutine(Brew());
        }

        private IEnumerator Brew()
        {
            _cauldronFill.color = Color.clear;
            _cauldronFillLeft.color = _ingredients[0].Attribute.Color;
            _cauldronFillRight.color = _ingredients[1].Attribute.Color;
            _brewTime = 0;

            while (_brewTime < BrewTime)
            {
                yield return null;
                _brewTime += Time.deltaTime;
            }

            _cauldronFill.sprite = _potFull;
            _cauldronFill.color = Color.Lerp(_ingredients[0].Attribute.Color, _ingredients[1].Attribute.Color, 0.5f);
            _cauldronFillLeft.color = Color.clear;
            _cauldronFillRight.color = Color.clear;
        }
    }
}
