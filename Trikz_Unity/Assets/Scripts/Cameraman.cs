using System.Collections;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public static Cameraman Instance { get; private set; }

    public Transform Target { get; set; }

    // Camera positions
    public readonly Vector3 _scenePos = new (2.5F, 9F, -15F);
    public readonly Quaternion _sceneRot = Quaternion.identity;

    public readonly Vector3 _launchablePos = new (0.35F, 2.7F, -4.58F);
    public readonly Quaternion _launchableRot = Quaternion.identity;

    public readonly Vector3 _wallPos = new (20.2889252f, 1.92013502f, -7.48522282f);
    public readonly Quaternion _wallRot = new (-0.0923758522f, -0.469788015f, -0.0495100021f, 0.876535654f);

    // Values for travellings
    public const float _xOffset = 2.5f;
    private Vector3 _vVelocity = Vector3.zero;
    float _lVelocity = 0.0f;

    // Running coroutines
    private Coroutine _zoomCoroutine = null;
    private Coroutine _moveCoroutine = null;


    public void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        GameManager.StateChanged += OnStateChanged;

        LaunchEvents.DampToggled += OnDampToggled;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;

        LaunchEvents.DampToggled -= OnDampToggled;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.State == GameState.Damping) // Follow bottle on x axis
        {
            Vector3 targetPosition = new Vector3(Target.position.x + _xOffset, _scenePos.y, _scenePos.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _vVelocity, .1f);
        }
    }
   
    private void OnDampToggled(bool damp)
    {
        StartSmoothZoom(.07f, damp ? 64 : 60);
    }

    private IEnumerator SmoothZoom(float zoomTime, float zoomTarget)
    {
        float t = 0; // Init time
        while (t < zoomTime) {
            t += Time.deltaTime; // Increment time
            // Update fov
            float fov = Camera.main.fieldOfView;
            Camera.main.fieldOfView = Mathf.SmoothDamp(fov, zoomTarget, ref _lVelocity, zoomTime);
            yield return null;
        }
    }

    private void StartSmoothZoom(float zoomTime, float zoomTarget)
    {
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(SmoothZoom(zoomTime, zoomTarget));
    }

    private IEnumerator SmoothMove(Vector3 newPos, Quaternion newRot, float mvtSpeed)
    {
        transform.GetPositionAndRotation(out Vector3 basePos, out Quaternion baseRot); // Init base position and rotation
        float sinTime = 0; // Init time
        while (sinTime != Mathf.PI) {
            // Increment time
            sinTime += Time.deltaTime * mvtSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            // Lerp sinTime to get a value between 0 and 1
            float lerpTime = SmoothMovement.SmoothSin(sinTime);
            // Apply transforms
            transform.SetPositionAndRotation(Vector3.Lerp(basePos, newPos, lerpTime),
                                             Quaternion.Lerp(baseRot, newRot, lerpTime));
            yield return null;
        }
    }

    private void StartSmoothMove (Vector3 newPos, Quaternion newRot, float mvtSpeed)
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(SmoothMove(newPos, newRot, mvtSpeed));
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Win:
            case GameState.Lose:
                StartSmoothMove(_wallPos, _wallRot, 4);
                return;
            case GameState.Launching:
                StartSmoothMove(_scenePos, _sceneRot, 4);
                return;
            case GameState.Rotating:
                StartSmoothMove(_launchablePos, _launchableRot, 4);
                return;
            case GameState.Damping:
                StopCoroutine(_moveCoroutine);
                return;
        }
    }
}
