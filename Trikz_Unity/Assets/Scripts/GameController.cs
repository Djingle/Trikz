using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static event Action<int> totalPointsChanged;
    public static event Action<int> currentPotChanged;
    public static event Action<int> highScoreChanged;
    public static event Action<float> multiplierChanged;

    private Launchable _launchable;

    private bool _isOver = false;

    private int _totalPoints = 0;
    private int _currentPot = 1;
    private int _multipliedPot = 1;
    private int _highScore = 0;
    private float _currentMultiplier = 1f;

    private InputController.LaunchState _launchState;


    private void Awake()
    {
        GameController[] controllers = FindObjectsOfType<GameController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
        _launchable = GameObject.FindObjectOfType<Launchable>();
    }

    private void OnEnable()
    {
        InputController.PhaseChanged += PhaseChanged;
        Launchable.hasStopped += OnLaunchableHasStopped;
        Launchable.hasFlipped += OnLaunchableHasFlipped;
    }

    private void OnDisable()
    {
        InputController.PhaseChanged -= PhaseChanged;
        Launchable.hasStopped -= OnLaunchableHasStopped;
        Launchable.hasFlipped -= OnLaunchableHasFlipped;
    }

    public void OnLaunchableHasStopped(bool win)
    {
        if (_launchable.transform.up.y > .9f)
        {
            _currentPot = _multipliedPot;
            currentPotChanged?.Invoke(_currentPot);
        }
        else
        {
            _currentPot = 1;
            currentPotChanged?.Invoke(_currentPot);
        }
        _isOver = true;
    }

    private void OnLaunchableHasFlipped()
    {
        _currentMultiplier *= 1.5f;
        _multipliedPot = (int)MathF.Ceiling(_currentPot * _currentMultiplier);

        currentPotChanged?.Invoke(_multipliedPot);
        multiplierChanged?.Invoke(_currentMultiplier);
    }

    public void Withdraw()
    {
        if (!_isOver) return;

        _totalPoints += _currentPot;
        if (_currentPot > _highScore)
        {
            _highScore = _currentPot;
            highScoreChanged?.Invoke(_highScore);
        }
        _currentPot = 1;

        totalPointsChanged.Invoke(_totalPoints);
        currentPotChanged.Invoke(_currentPot);
    }

    public void Replay()
    {
        if (!_isOver) return;
        _currentPot = 1;
        currentPotChanged.Invoke(_currentPot);
    }


    public void Launch(float height, float rotateStrength)
    {
        _launchable.Launch(height, rotateStrength);
        _multipliedPot = _currentPot;
        _currentMultiplier = 1;
    }



    private void PhaseChanged(InputController.LaunchState launchState)
    {
        _launchState = launchState;
    }

    public void Damp(bool isDamping)
    {
        _launchable.Damp(isDamping);
    }
}
