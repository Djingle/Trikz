using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System;

public class DampScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _button;

    public static event Action<bool> Damp;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _button = _root.Q("damp-button") as VisualElement;
        _button.RegisterCallback<PointerDownEvent>(OnClicked);
        _button.RegisterCallback<PointerUpEvent>(OnClickReleased);
    }

    private void OnClicked(PointerDownEvent evt)
    {
        Damp?.Invoke(true);
    }

    private void OnClickReleased(PointerUpEvent evt)
    {
        Damp?.Invoke(false);
    }
}