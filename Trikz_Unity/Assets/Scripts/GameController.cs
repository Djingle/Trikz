using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static event Action<int> totalPointsChanged;
    public static event Action<int> currentPotChanged;
    public static event Action<bool> launchEnded;

    public bool _isLaunchableMoving = false;
    private Draggable3D _launchable;
    private TargetController _tc;
    private Cameraman _cameraman;
    private UI _ui;
    private float _restCheckTimer = .5f;
    private bool _isOver = false;
    private int _totalPoints = 0;
    private int _currentPot = 1;

    private void Awake()
    {
        GameController[] controllers = FindObjectsOfType<GameController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
        _ui = GameObject.FindObjectOfType<UI>();
        _tc = GameObject.FindObjectOfType<TargetController>();
        _launchable = GameObject.FindObjectOfType<Draggable3D>();
        _cameraman = GameObject.FindObjectOfType<Cameraman>();
    }

    private void OnEnable()
    {
        UI.continueClicked += Continue;
        UI.replayClicked += Replay;
        UI.withdrawClicked += Withdraw;
    }

    private void OnDisable()
    {
        UI.continueClicked -= Continue;
        UI.replayClicked -= Replay;
        UI.withdrawClicked -= Withdraw;
    }

    void Update()
    {
        if (_isLaunchableMoving) // To move to launchable
        {
            _restCheckTimer -= Time.deltaTime;
            if (_restCheckTimer <= 0)
            {
                if (!_launchable.IsMoving())
                {
                    _isLaunchableMoving = false;
                    CheckWin();
                }
                else _restCheckTimer = .5f;
            }
        }
    }

    public void NewLaunch()
    {
        _isOver = false;
        _launchable.Reset();
        _tc.NewTarget();
        _cameraman.StartPreview();
    }

    public void CheckWin()
    {
        if (_launchable.IsUpRight() && _tc.CheckIn(_launchable.transform.position.x))
        {
            _currentPot *= 2;
            currentPotChanged?.Invoke(_currentPot);
            launchEnded?.Invoke(true);
            Debug.Log("C'est gagné bien joué voilà");
        }
        else
        {
            _currentPot *= 1;
            currentPotChanged?.Invoke(_currentPot);
            launchEnded?.Invoke(false);
            Debug.Log("Nul à chier le man (" + _currentPot + ")");
        }
        _isOver = true;
    }

    private void Continue()
    {
        if (!_isOver) return;
        NewLaunch();
    }

    public void Withdraw()
    {
        if (!_isOver) return;

        _totalPoints += _currentPot;
        _currentPot = 1;

        totalPointsChanged.Invoke(_totalPoints);
        currentPotChanged.Invoke(_currentPot);

        NewLaunch();
    }

    public void Replay()
    {
        if (!_isOver) return;
        _currentPot = 1;
        currentPotChanged.Invoke(_currentPot);
        NewLaunch();
    }

    public void Grab()
    {
        //Time.timeScale = 0.75f;
    }

    public void Launch()
    {
        _isLaunchableMoving = true;
        Time.timeScale = 1f;
        _cameraman.FocusScene();
    }
}
