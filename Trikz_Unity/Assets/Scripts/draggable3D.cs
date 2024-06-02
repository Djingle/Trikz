using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable3D : MonoBehaviour
{
    
    private Rigidbody _rb;
    private Vector3 _startPos = new Vector3(0f, 3f, 0f);
    private Quaternion _startRot = new Quaternion(0, 0, 0, 1);
    private float _mvtThreshold = .01f;

    private void Awake()
    {
        // Initialize RigidBody and Joint
        _rb = GetComponent<Rigidbody>();
    }


    public bool IsMoving()
    {
        return  _rb.velocity.x > _mvtThreshold ||
                _rb.velocity.y > _mvtThreshold ||
                _rb.angularVelocity.z > _mvtThreshold;
    }

    public bool IsUpRight()
    {
        return transform.up.y > .8f;
    }

    public void Reset()
    {
        transform.position = _startPos;
        transform.rotation = _startRot;
    }
}
