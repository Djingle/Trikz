using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LaunchScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Slider _slider;

    public static event Action<float> HeightChanged;
    public static event Action<float> HeightDecided;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _slider = _root.Q("angle-slider") as Slider;
        _slider.RegisterCallback<ChangeEvent<float>>(OnHeightChanged);

        var dragger = _slider.Q("unity-drag-container");
        dragger.RegisterCallback<PointerUpEvent>(OnHeightDecided);
        _slider.RegisterCallback<ClickEvent>(OnClick);
    }

    private void OnHeightChanged(ChangeEvent<float> value)
    {
        HeightChanged?.Invoke(value.newValue);
    }

    private void OnHeightDecided(PointerUpEvent evt)
    {
        HeightDecided?.Invoke(_slider.value);
    }

    private void OnClick(ClickEvent evt)
    {
        Debug.Log("click : " + evt.pointerType);
    }
}
