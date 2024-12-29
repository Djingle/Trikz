using UnityEngine;

public class Launchable : Bottle
{
    public static Launchable Instance { get; private set; }

    public float _endX = 14f;
    public float _launchSpeed = 160;
    public float _minHeight = 2f;
    public float _maxHeight = 10f;
    public float _animationRotateSpeed = 3;


    Rigidbody _rigidBody;
    BoxCollider _collider;
    
    Vector3 _displacment = Vector3.zero;
    float _sinTime = Mathf.PI;
    float _decidedLaunchHeight, _decidedRotateStrength;

    Vector3 _startPos = new (0, 2.3f, 0);
    Quaternion _startRot = Quaternion.identity;
    Quaternion _minAngle = Quaternion.Euler(0, 0, 0);
    Quaternion _maxAngle = Quaternion.Euler(0, 0, -30);
    Quaternion _launchAnimationAngle = Quaternion.Euler(0, 0, 20);

    Quaternion _currentRot, _targetRot;

    const float k_MvtThreshold = .0000001f;
    const float K_UpRightThreshold = .8f;
    bool _upsideDown = false;

    public GameObject _launchPreview;


    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        else {
            Instance = this;
        }

        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();

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
        _rigidBody.centerOfMass = new Vector3(0, BottleData.ComY, 0);
        // Init physics material
        PhysicMaterial material = new PhysicMaterial();
        material.staticFriction = BottleData.FrictionForce;
        material.dynamicFriction = BottleData.FrictionForce;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        _collider.material = material;
        // Init collider

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
            if (_displacment.magnitude < k_MvtThreshold)
            {
                if (transform.up.y > K_UpRightThreshold && transform.position.x > DoorController.instance.transform.position.x)
                    LaunchEvents.HasStopped?.Invoke(true);
                else
                    LaunchEvents.HasStopped?.Invoke(false);
                return;
            }
            _displacment = transform.position;

            // Tell InputController when launchable completes a flip
            if (!_upsideDown && transform.up.y < -K_UpRightThreshold) {
                _upsideDown = true;
            }
            if (_upsideDown && transform.up.y > -K_UpRightThreshold) {
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
        _rigidBody.useGravity = false;
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
       _rigidBody.angularDrag = isDamping ? BottleData.DampForce : 0f;
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
        _rigidBody.useGravity = true;
        height = Mathf.Lerp(_minHeight, _maxHeight, height);
        Vector3 force = new Vector3(_endX/(2f * height), height / 2f) * _launchSpeed;
        _rigidBody.AddForce(force);

        float maxTorque = BottleData.RotationForce;
        force = new Vector3(0f, 0f, -Mathf.Lerp(-maxTorque, maxTorque, rotateStrength));
        _rigidBody.AddTorque(force, ForceMode.VelocityChange);
    }
}
