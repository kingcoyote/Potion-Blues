using Sirenix.OdinInspector;
using UnityEngine;

public class DispenserScript : MonoBehaviour
{
    [OnValueChanged("UpdateColor")]
    public Color IngredientColor;
    public IngredientScript IngredientPrefab;

    public void OnMouseDown()
    {
        var ingredient = Instantiate(IngredientPrefab);
        ingredient.Color = IngredientColor;
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = IngredientColor;
    }
}
