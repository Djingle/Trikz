using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{

    [SerializeField] private Camera _cam;
    [SerializeField] private Launchable _launchable;

    private Vector3 _scenePos = new Vector3(2.5F, 9F, -15F);
    private Quaternion _sceneRot = Quaternion.identity;

    private Vector3 _launchablePos = new Vector3(0.35F, 2.7F, -4.58F);
    private Quaternion _launchableRot = Quaternion.identity;

    private Vector3 _wallPos = new Vector3(20.2889252f, 1.92013502f, -7.48522282f);
    private Quaternion _wallRot = new Quaternion(-0.0923758522f, -0.469788015f, -0.0495100021f, 0.876535654f);

    private Vector3 _targetPos, _currentPos;
    private Quaternion _targetRot, _currentRot;

    private float _mvtSpeed = 5f;
    private float _sinTime = 4f;
    private bool _followLaunchable = false;
    public float _xOffset = 2.5f;
    private Vector3 velocity = Vector3.zero;

    private Transform _target;


    private void OnEnable()
    {
        InputController.PhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        InputController.PhaseChanged -= OnPhaseChanged;
    }

    private void Start()
    {
        _target = _launchable.transform.GetChild(0);
    }

    private void LateUpdate()
    {
        if (_sinTime != Mathf.PI)
        {
            _sinTime += Time.deltaTime * _mvtSpeed;
            _sinTime = Mathf.Clamp(_sinTime, 0, Mathf.PI);
            updateMove();
        }

            if (_followLaunchable)
        {
            Vector3 targetPosition = new Vector3(_target.position.x + _xOffset, _scenePos.y, _scenePos.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, .1f);
        }
    }

    private void Move(Vector3 newPos, Quaternion newRot)
    {
        _currentPos = transform.position;
        _currentRot = transform.rotation;
        _targetPos = newPos;
        _targetRot = newRot;
        _sinTime = 0;
    }

    private void updateMove()
    {
        float lerpTime = SmoothMovement.SmoothSin(_sinTime);
        transform.SetPositionAndRotation(Vector3.Lerp(_currentPos, _targetPos, lerpTime),
                                         Quaternion.Lerp(_currentRot, _targetRot, lerpTime));
    }

    private void OnPhaseChanged(InputController.LaunchState launchState)
    {
        switch (launchState)
        {
            case InputController.LaunchState.End:
                Move(_wallPos, _wallRot);
                _followLaunchable = false;
                return;
            case InputController.LaunchState.Launching:
                Move(_scenePos, _sceneRot);
                return;
            case InputController.LaunchState.Rotating:
                Move(_launchablePos, _launchableRot);
                return;
            case InputController.LaunchState.Damping:
                _followLaunchable = true;
                return;
        }
    }
}
