using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class InputController : MonoBehaviour
{
    public static event Action<LaunchState> PhaseChanged;

    public enum LaunchState {
        End,
        Launching,
        Rotating,
        Damping
    }

    private LaunchState _launchState = LaunchState.Launching;
    private GameController _gc;
    private float _height, _rotateStrength;



    // Start is called before the first frame update
    void Start()
    {
        _gc = GameObject.FindObjectOfType<GameController>();
        ChangePhase(LaunchState.Launching);

    }

    private void OnEnable()
    {
        LaunchScreenScript.HeightDecided += OnHeightDecided;
        RotateScreenScript.StrengthDecided += OnStrengthDecided;
        DampScreenScript.Damp += Damp;

        WinScreenScript.continueClicked += OnContinue;
        WinScreenScript.withdrawClicked += OnWithdraw;
        LoseScreenScript.replayClicked += OnReplay;

        Launchable.hasStopped += OnLaunchEnded;
    }

    private void OnDisable()
    {
        LaunchScreenScript.HeightDecided -= OnHeightDecided;
        RotateScreenScript.StrengthDecided -= OnStrengthDecided;
        DampScreenScript.Damp -= Damp;

        WinScreenScript.continueClicked -= OnContinue;
        WinScreenScript.withdrawClicked -= OnWithdraw;
        LoseScreenScript.replayClicked -= OnReplay;

        Launchable.hasStopped -= OnLaunchEnded;
    }

    private void OnContinue()
    {
        ChangePhase(LaunchState.Launching);
    }

    private void OnWithdraw()
    {
        ChangePhase(LaunchState.Launching);
        _gc.Withdraw();
    }
    private void OnReplay()
    {
        ChangePhase(LaunchState.Launching);
        _gc.Replay();
    }

    private void OnHeightDecided(float height)
    {
        _height = height;
        ChangePhase(LaunchState.Rotating);
    }

    private void OnStrengthDecided(float strength)
    {
        _rotateStrength = strength;
        _gc.Launch(_height, _rotateStrength);
        ChangePhase(LaunchState.Damping);
    }

    private void Damp(bool isPressed)
    {
        _gc.Damp(isPressed);
    }

    private void OnLaunchEnded(bool win)
    {
        ChangePhase(LaunchState.End);
    }

    private void ChangePhase(LaunchState launchState)
    {
        _launchState = launchState;
        PhaseChanged?.Invoke(_launchState);
    }

}
