using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class RotateScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Slider _slider;

    public static event Action<float> StrengthChanged;
    public static event Action<float> StrengthDecided;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _slider = _root.Q("rotate-slider") as Slider;
        _slider.RegisterCallback<ChangeEvent<float>>(OnStrengthChanged);

        var dragger = _slider.Q("unity-drag-container");
        dragger.RegisterCallback<PointerUpEvent>(OnStrengthDecided);

    }

    private void OnStrengthChanged(ChangeEvent<float> value)
    {
        StrengthChanged?.Invoke(value.newValue);
    }

    private void OnStrengthDecided(PointerUpEvent evt)
    {
        StrengthDecided?.Invoke(_slider.value);
    }
}
