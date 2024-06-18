using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LoseScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Button _replayButton;

    public static event Action replayClicked;

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _replayButton = _root.Q("replay-button") as Button;
        _replayButton.RegisterCallback<ClickEvent>(onReplay);
    }

    private void OnDisable()
    {
        _replayButton.UnregisterCallback<ClickEvent>(onReplay);
    }

    public void onReplay(ClickEvent evt)
    {
        replayClicked.Invoke();
    }
}
