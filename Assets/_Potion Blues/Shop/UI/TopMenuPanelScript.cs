using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopMenuPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _version;

    // Start is called before the first frame update
    void Start()
    {
        _version.text = $"Version {Application.version}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
