using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UI : MonoBehaviour
{
    public GameObject _winScreen;
    public GameObject _loseScreen;

    public GameObject _launchScreen;
    public GameObject _rotateScreen;
    public GameObject _dampScreen;

    private GameObject _endScreen, _controlScreen;

    private VisualElement _root;

    private Label _totalPoints, _currentPot, _highScore, _multiplier;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _totalPoints = _root.Q("total-points-value") as Label;
        _currentPot = _root.Q("current-pot-value") as Label;
        _highScore = _root.Q("high-score-value") as Label;
        _multiplier = _root.Q("multiplier-value") as Label;
        _totalPoints.text = "0";
        _currentPot.text = "0";
    }

    private void OnEnable()
    {
        GameController.totalPointsChanged += UpdateTotalPoints;
        GameController.currentPotChanged += UpdateCurrentPot;
        GameController.highScoreChanged += UpdateHighScore;
        GameController.multiplierChanged += UpdateMultiplier;

        Launchable.hasStopped += ShowEndScreen;

        WinScreenScript.withdrawClicked += ClearEndScreen;
        WinScreenScript.continueClicked += ClearEndScreen;
        LoseScreenScript.replayClicked += ClearEndScreen;

        InputController.PhaseChanged += ChangeControlScreen;
     }

    private void OnDisable()
    {
        GameController.totalPointsChanged -= UpdateTotalPoints;
        GameController.currentPotChanged -= UpdateCurrentPot;
        GameController.highScoreChanged -= UpdateHighScore;
        GameController.multiplierChanged -= UpdateMultiplier;

        Launchable.hasStopped -= ShowEndScreen;

        WinScreenScript.withdrawClicked -= ClearEndScreen;
        WinScreenScript.continueClicked -= ClearEndScreen;
        LoseScreenScript.replayClicked -= ClearEndScreen;

        InputController.PhaseChanged -= ChangeControlScreen;

    }

    private void UpdateTotalPoints(int newPoints)
    {
        _totalPoints.text = newPoints.ToString();
    }

    private void UpdateCurrentPot(int newPot)
    {
        _currentPot.text = newPot.ToString();
    }

    private void UpdateHighScore(int newHighScore)
    {
        _highScore.text = newHighScore.ToString();
    }

    private void UpdateMultiplier(float newMultiplier)
    {
        _multiplier.text = newMultiplier.ToString("0.##");
    }

    private void ShowEndScreen(bool win)
    {
        if (win)
        {
            _endScreen = GameObject.Instantiate(_winScreen);
        }
        else
        {
            _endScreen = GameObject.Instantiate(_loseScreen);
        }
    }

    private void ClearEndScreen()
    {
        UpdateMultiplier(1);
        Destroy(_endScreen);
    }


    private void ChangeControlScreen(InputController.LaunchState launchState)
    {
        if (_controlScreen != null) Destroy(_controlScreen);

        switch (launchState)
        {
            case InputController.LaunchState.Launching :
                _controlScreen = GameObject.Instantiate(_launchScreen);
                break;
            case InputController.LaunchState.Rotating :
                _controlScreen = GameObject.Instantiate(_rotateScreen);
                break;
            case InputController.LaunchState.Damping :
                _controlScreen = GameObject.Instantiate(_dampScreen);
                break;
        }
    }
}
