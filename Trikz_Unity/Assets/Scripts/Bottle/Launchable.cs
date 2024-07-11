using UnityEngine;
using System;

public class Launchable : Bottle
{
    public static Launchable Instance { get; private set; }

    private const float _endX = 17.5f;
    private const float _launchSpeed = 175;
    private const float _minHeight = 2f;
    private const float _maxHeight = 10f;
    private const float _animationRotateSpeed = 3;

    private Rigidbody _rb;
    private Vector3 _displacment = Vector3.zero;
    private float _sinTime = Mathf.PI;
    private float _decidedLaunchHeight, _decidedRotateStrength;

    private Vector3 _startPos = new (0, 2.3f, 0);
    private Quaternion _startRot = Quaternion.identity;
    private Quaternion _minAngle = Quaternion.Euler(0, 0, 0);
    private Quaternion _maxAngle = Quaternion.Euler(0, 0, -30);
    private Quaternion _launchAnimationAngle = Quaternion.Euler(0, 0, 20);

    private Quaternion _currentRot, _targetRot;

    private const float _mvtThreshold = .0000001f;
    private bool _upsideDown = false;
    private float _upRightThreshhold = .8f;

    public GameObject _launchPreview;

    private Transform _p;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        else {
            Instance = this;
        }

        _rb = GetComponent<Rigidbody>();

        // Instanciate previews
        _launchPreview = Instantiate(_launchPreview, transform);
        _launchPreview.SetActive(false);
        Reset();
    }

    private void OnEnable()
    {
        GameManager.StateChanged += OnStateChanged;

        LaunchEvents.HeightChanged += RotateForHeight;
        LaunchEvents.HeightDecided += OnHeightDecided;
        LaunchEvents.StrengthChanged += RotateForStrength;
        LaunchEvents.StrengthDecided += OnRotateStrengthDecided;
        LaunchEvents.DampToggled += OnDampToggled;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;

        LaunchEvents.HeightChanged -= RotateForHeight;
        LaunchEvents.HeightDecided -= OnHeightDecided;
        LaunchEvents.StrengthChanged -= RotateForStrength;
        LaunchEvents.StrengthDecided -= OnRotateStrengthDecided;
        LaunchEvents.DampToggled -= OnDampToggled;
    }

    public override void Init(BottleData bottleData)
    {
        base.Init(bottleData);
        // Hide CoM and expose it to camera
        CenterOfMass.gameObject.SetActive(false);
        Cameraman.Instance.Target = CenterOfMass.transform;
        // Init rigidBody CoM
        _rb.centerOfMass = new Vector3(0, BottleData.ComY, 0);
        // Init Collider
        MeshCollider collider = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        if (_sinTime != Mathf.PI)
        {
            _sinTime += Time.deltaTime * _animationRotateSpeed;
            _sinTime = Mathf.Clamp(_sinTime, 0, Mathf.PI);
            updateRotation();
        }

        // Only when launchable has been launched
        if (GameManager.Instance.State == GameState.Damping)
        {
            _displacment -= transform.position;
            if (_displacment.magnitude < _mvtThreshold)
            {
                if (transform.up.y > _upRightThreshhold && transform.position.x > DoorController.instance.transform.position.x)
                    LaunchEvents.HasStopped?.Invoke(true);
                else
                    LaunchEvents.HasStopped?.Invoke(false);
                return;
            }
            _displacment = transform.position;

            // Tell InputController when launchable completes a flip
            if (!_upsideDown && transform.up.y < -_upRightThreshhold) {
                _upsideDown = true;
            }
            if (_upsideDown && transform.up.y > -_upRightThreshhold) {
                _upsideDown = false;
                LaunchEvents.HasFlipped?.Invoke();
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
        transform.SetPositionAndRotation(_startPos, _startRot);
        _rb.useGravity = false;
        _upsideDown = false;
        OnDampToggled(false);
    }

    private void RotateForHeight(float time)
    {
        transform.rotation = Quaternion.Lerp(_minAngle, _maxAngle, time);
    }
    private void RotateForStrength(float time)
    {
        //Debug.Log("rotateForStrength");
    }

    private void OnHeightDecided(float height)
    {
        _decidedLaunchHeight = height;
    }

    private void OnRotateStrengthDecided(float strength)
    {
        _decidedRotateStrength = strength;
        Launch(_decidedLaunchHeight, _decidedRotateStrength);
    }

    private void OnDampToggled(bool isDamping)
    {
       _rb.angularDrag = isDamping ? BottleData.DampingFactor : 0f;
    }

    private void OnStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Launching:
                if (!_launchPreview.activeSelf) _launchPreview.SetActive(true);
                else Debug.Log("Launchable : Preview already active");
                Reset();
                return;


            case GameState.Rotating:
                if (_launchPreview.activeSelf) _launchPreview.SetActive(false);
                else Debug.Log("Launchable : Preview not active");

                SmoothRotate(_launchAnimationAngle);
                return;
            case GameState.Menu:
                Destroy(gameObject);
                return;
        }
    }

    public void Launch(float height, float rotateStrength)
    {
        _sinTime = Mathf.PI;
        _rb.useGravity = true;
        height = Mathf.Lerp(_minHeight, _maxHeight, height);
        Vector3 force = new Vector3(_endX/(2f * height), height / 2f) * _launchSpeed;
        _rb.AddForce(force);

        float maxTorque = BottleData.RotationForce;
        force = new Vector3(0f, 0f, -Mathf.Lerp(-maxTorque, maxTorque, rotateStrength));
        _rb.AddTorque(force, ForceMode.VelocityChange);
    }
}
