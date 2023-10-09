using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float SpawnRate;
    [SerializeField] private CustomerScript _customerPrefab;
    private BoxCollider2D _entrance;
    private float _nextSpawn;
    private List<Color> _colors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        _entrance = GetComponent<BoxCollider2D>();
        _nextSpawn = SpawnRate;

        var dispensers = FindObjectsOfType<DispenserScript>();
        foreach (var dispenser in dispensers)
        {
            _colors.Add(dispenser.IngredientColor);
        }
        for (var i = 0; i < dispensers.Length; i++)
        {
            for (var j = 0;  j < dispensers.Length; j++)
            {
                var newColor = Color.Lerp(_colors[i], _colors[j], 0.5f);
                if (_colors.Contains(newColor)) continue;
                _colors.Add(newColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _nextSpawn -= Time.deltaTime;
        if (_nextSpawn < 0)
        {
            SpawnRate *= 0.99f;
            var customer = Instantiate(_customerPrefab);
            customer.transform.position = transform.position + Vector3.down * Random.Range(0, _entrance.size.y);
            customer.Color = _colors[Random.Range(0, _colors.Count)];
            _nextSpawn = SpawnRate;
        }
    }
}
