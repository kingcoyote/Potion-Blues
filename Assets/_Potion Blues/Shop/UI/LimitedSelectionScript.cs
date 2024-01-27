using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LimitedSelectionScript : MonoBehaviour
{
    public int MaximumSelection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaximumSelection(int max)
    {
        MaximumSelection = max;
        var toggleChildren = GetComponentsInChildren<LeanToggle>()
            .Where(x => x.gameObject.IsDestroyed() == false)
            .ToList();

        for (int i = 0; i < toggleChildren.Count; i++)
        {
            toggleChildren[i].Set(i < MaximumSelection);
            toggleChildren[i].OnOn.AddListener(UpdateToggles);
            toggleChildren[i].OnOff.AddListener(UpdateToggles);
        }
    }

    public void UpdateToggles()
    {
        // count enabled toggles. if less than max, set all to interactable.
        // if equal to max, set all disabled to non interactable.
    }
}
