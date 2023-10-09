using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronScript : MonoBehaviour
{
    public float BrewTime;
    public int MaxIngredients;

    private int _ingredients;
    private float _brewTime;
    private CauldronState _state = CauldronState.Idle;

    [SerializeField] private Image _ingredientIcon;
    [SerializeField] private Image _progress;
    [SerializeField] private PotionScript _potionPrefab;

    private void Update()
    {
        switch (_state)
        {
            case CauldronState.Idle:
                
                break;
            case CauldronState.Brewing:
                _brewTime -= Time.deltaTime;
                if (_brewTime < 0)
                {
                    _brewTime = 0;
                    _state = CauldronState.Ready;
                }
                _progress.fillAmount = (BrewTime - _brewTime) / BrewTime;
                break;
            case CauldronState.Ready:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ingredient = collision.gameObject.GetComponent<IngredientScript>();

        if (ingredient == null || _ingredients >= MaxIngredients) return;

        AddIngredient(ingredient);

        Destroy(ingredient.gameObject);
    }

    private void AddIngredient(IngredientScript ingredient)
    {
        if (_ingredients < MaxIngredients)
        {
            var newIcon = Instantiate(_ingredientIcon, _ingredientIcon.transform.parent);
            newIcon.gameObject.SetActive(true);
            newIcon.name = "ingredient";
            var image = newIcon.GetComponent<Image>();
            image.color = ingredient.Color;
            image.fillAmount = (1.0f / MaxIngredients);
            image.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (float)_ingredients / MaxIngredients * 360.0f));

            _ingredients++;
        }

        if (_ingredients == MaxIngredients)
        {
            StartBrewing();
        }
    }

    public void OnMouseDown()
    {
        if (_state != CauldronState.Ready) return;

        _state = CauldronState.Idle;
        _progress.fillAmount = 0;

        var potion = Instantiate(_potionPrefab);
        potion.Color = _progress.color;
    }

    private void StartBrewing()
    {
        _state = CauldronState.Brewing;
        _ingredients = 0;
        _brewTime = BrewTime;

        List<Color> ingredientColors = new List<Color>();

        for(var i = _ingredientIcon.transform.parent.childCount - 1; i >= 0 ; i--)
        {
            var j = _ingredientIcon.transform.parent.GetChild(i);
            if (j.name != "ingredient") continue;

            ingredientColors.Add(j.GetComponent<Image>().color);

            Destroy(j.gameObject);
        }

        float[] potionColor = new float[3] { 0, 0, 0 };

        foreach (var color in ingredientColors)
        {
            potionColor[0] += color.r;
            potionColor[1] += color.g;
            potionColor[2] += color.b;
        }

        _progress.color = new Color(
            potionColor[0] / ingredientColors.Count, 
            potionColor[1] / ingredientColors.Count, 
            potionColor[2] / ingredientColors.Count
        );
    }

    private enum CauldronState
    {
        Idle,
        Brewing,
        Ready
    }
}
