using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{

    [SerializeField] private Camera cam;

    // Scene camera projection matrix, position and rotation
    private Matrix4x4 _sceneMat = new Matrix4x4(new Vector4(1.71791f, 0f, 0f, 0f),
                                                   new Vector4(0f, 0.96569f, 0f, 0f),
                                                   new Vector4(0f, 0f, -1.0060f,-1f),
                                                   new Vector4(0f,0f,-0.60018f,0f));
    private Vector3 _scenePos = new Vector3(-1.615942f, 11.5648022f, -10.216013f);
    private Quaternion _sceneRot = new Quaternion(-0.257784307f, -0.340780735f, 0.0977412313f, -0.898811638f);

    // Launchable camera projection matrix, position and rotation
    private Matrix4x4 _launchableMat = new Matrix4x4(new Vector4(0.35579f, 0f, 0f, 0f),
                                                        new Vector4(0f, 0.2f, 0f, 0f),
                                                        new Vector4(0f, 0f, -0.02f, 0f),
                                                        new Vector4(0f, 0f, -1.006f, 1f));
    private Vector3 _launchablePos = new Vector3(0f, 4.58f, -10f);
    private Quaternion _launchableRot = new Quaternion(0f, 0f, 0f, 1f);

    private Vector3 _targetPos, _currentPos;
    private Quaternion _targetRot, _currentRot;
    private Matrix4x4 _currentMat, _targetMat;

    private float _mvtSpeed = 4f;
    private float _sinTime = Mathf.PI;
    private float _lerpTime;
    private float _scenePreviewTime = 2f;
    private bool _isPreviewOn = true;

    void Start()
    {
        StartPreview();
    }

    void Update()
    {
        if (_isPreviewOn) {
            if (_scenePreviewTime > 0) _scenePreviewTime -= Time.deltaTime;
            else stopPreview();
        }

        // Traveling (only travel if current position != target position)
        if (_sinTime != Mathf.PI)
        {
            _sinTime += Time.deltaTime * _mvtSpeed;
            _sinTime = Mathf.Clamp(_sinTime, 0, Mathf.PI);
            _lerpTime = SmoothSin(_sinTime);
            transform.position = Vector3.Lerp(_currentPos, _targetPos, _lerpTime);
            transform.rotation = Quaternion.Lerp(_currentRot, _targetRot, _lerpTime);
            cam.projectionMatrix = MatrixLerp(_currentMat, _targetMat, _lerpTime);
        }
    }

    public void stopPreview()
    {
        _isPreviewOn = false;
        _scenePreviewTime = 4f;
        FocusLaunchable();
    }

    public void StartPreview()
    {
        _scenePreviewTime = 4f;
        _isPreviewOn = true;
        FocusScene();
    }

    public void FocusScene()
    {
        _sinTime = 0f;
        _currentMat = _launchableMat;
        _currentPos = _launchablePos;
        _currentRot = _launchableRot;
        _targetMat = _sceneMat;
        _targetPos = _scenePos;
        _targetRot = _sceneRot;
    }

    public void FocusLaunchable()
    {
        _sinTime = 0f;
        _currentMat = _sceneMat;
        _currentPos = _scenePos;
        _currentRot = _sceneRot;
        _targetMat = _launchableMat;
        _targetPos = _launchablePos;
        _targetRot = _launchableRot;
    }

    private float SmoothSin(float x)
    {
        return .5f * Mathf.Sin(x - Mathf.PI / 2f) + .5f;
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }
}
