using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class WinScreenScript : MonoBehaviour
{
    private VisualElement _root;
    private Button _continueButton;
    private Button _withdrawButton;

    public static event Action continueClicked;
    public static event Action withdrawClicked;


    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _continueButton = _root.Q("continue-button") as Button;
        _withdrawButton = _root.Q("withdraw-button") as Button;
        _continueButton.RegisterCallback<ClickEvent>(onContinue);
        _withdrawButton.RegisterCallback<ClickEvent>(onWithdraw);
    }

    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(onContinue);
        _withdrawButton.UnregisterCallback<ClickEvent>(onWithdraw);
    }

    public void onContinue(ClickEvent evt)
    {
        continueClicked.Invoke();
    }

    public void onWithdraw(ClickEvent evt)
    {
        withdrawClicked.Invoke();

    }
}