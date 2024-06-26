using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Launchable : MonoBehaviour
{
    public static Launchable instance;

    public static event Action<bool> hasStopped;
    public static event Action hasFlipped;

    private Rigidbody _rb;
    private Vector3 _displacment;
    private float _sinTime = Mathf.PI;
    private float _rotateSpeed = 3;

    private Vector3 _startPos = new Vector3(0, 2.3f, 0);
    private Quaternion _startRot = new Quaternion(0, 0, 0, 1);
    private Quaternion _minAngle = Quaternion.Euler(0, 0, 0);
    private Quaternion _maxAngle = Quaternion.Euler(0, 0, -30);
    private Quaternion _launchAngle = Quaternion.Euler(0, 0, 20);

    private Quaternion _currentRot, _targetRot;

    private float _mvtThreshold = .0000001f;
    private bool _checkMoving = false;
    private bool _upsideDown = false;
    private float _upRightThreshhold = .8f;

    [SerializeField] private float _endX = 17.5f;
    [SerializeField] private float _launchSpeed = 175;
    [SerializeField] private float _minHeight = 2f;
    [SerializeField] private float _maxHeight = 10f;
    [SerializeField] private float _minTorque = -25f;
    [SerializeField] private float _maxTorque = 25f;
    [SerializeField] private float _baseDampingFactor = 0f;
    [SerializeField] private float _dampingFactor = 2f;

    public GameObject _launchPreview;
    //public GameObject _rotatePreview;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _rb = GetComponent<Rigidbody>();

        // Instanciate previews
        _launchPreview = Instantiate(_launchPreview, _startPos, Quaternion.Euler(0F, 0F, 0f));
        _launchPreview.SetActive(false);

        //_rotatePreview = Instantiate(_rotatePreview, transform);


        _displacment = Vector3.zero;
        Reset();
    }

    private void OnEnable()
    {
        InputController.PhaseChanged += OnPhaseChanged;
        LaunchScreenScript.HeightChanged += RotateForHeight;
        RotateScreenScript.StrengthChanged += RotateForStrength;
    }

    private void OnDisable()
    {
        InputController.PhaseChanged -= OnPhaseChanged;
        LaunchScreenScript.HeightChanged -= RotateForHeight;
        RotateScreenScript.StrengthChanged -= RotateForStrength;
    }

    private void FixedUpdate()
    {

        if (_sinTime != Mathf.PI)
        {
            _sinTime += Time.deltaTime * _rotateSpeed;
            _sinTime = Mathf.Clamp(_sinTime, 0, Mathf.PI);
            updateRotation();
        }

        // Only when launchable has been launched
        if (_checkMoving)
        {
            // Check if the displacement is 0
            _displacment -= transform.position;
            if (_displacment.magnitude < _mvtThreshold)
            {
                _checkMoving = false;
                if (transform.up.y > _upRightThreshhold && transform.position.x > DoorController.instance.transform.position.x)
                    hasStopped?.Invoke(true);
                else
                    hasStopped?.Invoke(false);
            }
            _displacment = transform.position;

            // Tell InputController when launchable completes a flip
            if (!_upsideDown && transform.up.y < -_upRightThreshhold)
            {
                _upsideDown = true;
            }
            if (_upsideDown == true && transform.up.y > -_upRightThreshhold)
            {
                _upsideDown = false;
                hasFlipped?.Invoke();
            }
        }
    }

    private void SmoothRotate(Quaternion newRot)
    {
        _currentRot = transform.rotation;
        _targetRot = newRot;
        _sinTime = 0;
    }

    private void updateRotation()
    {
        float lerpTime = SmoothMovement.SmoothSin(_sinTime);
        transform.rotation = Quaternion.Lerp(_currentRot, _targetRot, lerpTime);
    }

    public void Reset()
    {
        transform.position = _startPos;
        transform.rotation = _startRot;
        _rb.useGravity = false;
        _upsideDown = false;
        Damp(false);
    }

    private void RotateForHeight(float time)
    {
        transform.rotation = Quaternion.Lerp(_minAngle, _maxAngle, time);
    }
    private void RotateForStrength(float time)
    {
        //Debug.Log("rotateForStrength");
    }

    private void OnPhaseChanged(InputController.LaunchState launchState)
    {
        switch (launchState)
        {
            case InputController.LaunchState.Launching:
                if (!_launchPreview.activeSelf)
                {
                    _launchPreview.SetActive(true);
                }
                else Debug.Log("Launchable : Preview already active");
                Reset();
                return;


            case InputController.LaunchState.Rotating:
                if (_launchPreview.activeSelf)
                {
                    _launchPreview.SetActive(false);
                }
                else Debug.Log("Launchable : Preview not active");
                //if (!_rotatePreview.activeSelf)
                //{
                //    _rotatePreview.SetActive(true);
                //}
                //else Debug.Log("Launchable : Preview already active");
                SmoothRotate(_launchAngle);
                return;


            case InputController.LaunchState.Damping:
                //if (_rotatePreview.activeSelf)
                //{
                //    _rotatePreview.SetActive(false);
                //}
                //else Debug.Log("Launchable : Preview not active");
                return;
        }
    }

    public void Launch(float height, float rotateStrength)
    {
        _rb.useGravity = true;
        height = Mathf.Lerp(_minHeight, _maxHeight, height);
        Vector3 force = new Vector3(_endX/(2f * height), height / 2f) * _launchSpeed;
        _rb.AddForce(force);

        force = new Vector3(0f, 0f, -Mathf.Lerp(_minTorque, _maxTorque, rotateStrength));
        _rb.AddTorque(force, ForceMode.VelocityChange);

        _checkMoving = true;
    }

    public void Damp(bool isDamping)
    {
        _rb.angularDrag = isDamping ? _dampingFactor : _baseDampingFactor;
    }
}
