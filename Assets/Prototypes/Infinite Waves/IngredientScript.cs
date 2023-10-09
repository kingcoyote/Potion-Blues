using UnityEngine;

public class IngredientScript : MonoBehaviour
{
    public Color Color;

    public void Start()
    {
        GetComponent<SpriteRenderer>().color = Color;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    public void OnMouseUp()
    {
        Destroy(gameObject);
    }
}
