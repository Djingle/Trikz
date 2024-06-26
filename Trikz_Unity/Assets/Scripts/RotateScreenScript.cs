using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class RotateScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Slider _slider;
    private float _rootWidth, _minX, _maxX;
    private float _marginSize = .05f;

    public static event Action<float> StrengthChanged;
    public static event Action<float> StrengthDecided;


    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        //_slider = _root.Q("rotate-slider") as Slider;
        //_slider.RegisterCallback<ChangeEvent<float>>(OnStrengthChanged);

        //var dragger = _slider.Q("unity-drag-container");
        //dragger.RegisterCallback<PointerUpEvent>(OnStrengthDecided);

        _root.RegisterCallback<PointerMoveEvent>(OnMoved);
        _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        InitXs(_root.layout.width);
    }

    private void InitXs(float rootWidth)
    {
        _rootWidth = rootWidth;
        _minX = rootWidth * _marginSize;
        _maxX = rootWidth - (rootWidth * _marginSize);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        InitXs(_root.layout.width);
    }

    private float RemapValue(float value)
    {
        return Mathf.InverseLerp(_minX, _maxX, value);
    }

    private void OnMoved(PointerMoveEvent evt)
    {
        StrengthChanged?.Invoke(RemapValue(evt.position.x));
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        StrengthDecided?.Invoke(RemapValue(evt.position.x));
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
