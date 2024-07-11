using UnityEngine;

public class BottlePreview : Bottle
{
    [SerializeField] Vector3 _baseRotation = new Vector3(0, 0, 15);
    [SerializeField] Vector3 _rotationSpeed = new Vector3(0, 20, 0);
    [SerializeField] float _angle;
    float _rotationTime;
    Transform _p;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(_baseRotation);
        _baseRotation = new Vector3(0, 0, 15);
        _rotationSpeed = new Vector3(0, 20, 0);
    }

    private void Update()
    {
        _rotationTime += Time.deltaTime;
        Vector3 newRot = _baseRotation + _rotationSpeed * _rotationTime;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
