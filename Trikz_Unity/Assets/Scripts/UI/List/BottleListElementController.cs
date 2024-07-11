using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class BottleListElementController
{
    Label _label;

    // This function retrieves a reference to the 
    // character name label inside the UI element.
    public void SetVisualElement(VisualElement visualElement)
    {
        _label = visualElement.Q<Label>("bottle-name");
    }

    // This function receives the character whose name this list 
    // element is supposed to display. Since the elements list 
    // in a `ListView` are pooled and reused, it's necessary to 
    // have a `Set` function to change which character's data to display.
    public void SetBottleData(BottleData bottleData)
    {
        _label.text = bottleData.Name;
    }
}
