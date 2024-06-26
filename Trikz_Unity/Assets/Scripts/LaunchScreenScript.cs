using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LaunchScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Slider _slider;
    private float _rootHeight, _minY, _maxY;
    private float _marginSizeBottom = .5f;
    private float _marginSizeTop = .05f;

    public static event Action<float> HeightChanged;
    public static event Action<float> HeightDecided;


    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        //_slider = _root.Q("angle-slider") as Slider;
        //_slider.RegisterCallback<ChangeEvent<float>>(OnHeightChanged);

        //var dragger = _slider.Q("unity-drag-container");
        //dragger.RegisterCallback<PointerUpEvent>(OnHeightDecided);
        //_slider.RegisterCallback<ClickEvent>(OnClick);
        _root.RegisterCallback<PointerMoveEvent>(OnMoved);
        _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        InitYs(_root.layout.height);
    }

    private void InitYs(float rootHeight)
    {
        _rootHeight = rootHeight;
        _minY = rootHeight * _marginSizeBottom;
        _maxY = rootHeight - (rootHeight * _marginSizeTop);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        InitYs(_root.layout.height);
        Debug.Log("geometry changed");
    }


    private float RemapValue(float value)
    {
        Debug.Log("value : " + (Mathf.InverseLerp(_minY, _maxY, value)));
        return 1 - Mathf.InverseLerp(_minY, _maxY, value);
    }

    private void OnMoved(PointerMoveEvent evt)
    {
        HeightChanged?.Invoke(RemapValue(evt.position.y));
        Debug.Log("y: " + evt.position.y);
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        HeightDecided?.Invoke(RemapValue(evt.position.y));
        Debug.Log("décidé");
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
