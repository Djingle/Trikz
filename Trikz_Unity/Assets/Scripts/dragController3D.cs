using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool _isDragActive = false;
    private Vector2 _screenPos;
    private Vector3 _worldPos;
    private Draggable3D _lastDragged;
    private Rigidbody2D _rb;
    private float _mvSpeed = 10f;
    private ConfigurableJoint _draggableJoint;
    private RaycastHit _hit;
    private JointDrive _freeDrive;
    private JointDrive _attachedDrive;
    private JointDrive _attachedAngularDrive;
    private Vector3 _grabRay = new Vector3(0,0,1000);
    private Vector3 _dragTarget = Vector3.zero;

    void Awake() {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1) {
            Destroy(gameObject);
        }

        // Initialize free and attached drives
        _freeDrive.positionSpring = 0;
        _freeDrive.positionDamper = 0;
        _freeDrive.maximumForce = 0;
        _attachedDrive.positionSpring = 1000;
        _attachedDrive.positionDamper = 25;
        _attachedDrive.maximumForce = Mathf.Infinity;
        _attachedAngularDrive.positionDamper = 3;
        _attachedAngularDrive.positionSpring = 0;
        _attachedAngularDrive.maximumForce = Mathf.Infinity;
    }

    void Update()
    {
        if (_isDragActive && (Input.GetMouseButtonUp(0) || (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Ended))) {
            Drop();
            return;
        }

        if (Input.GetMouseButton(0)) {
            Vector3 mousePos = Input.mousePosition;
            _screenPos = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0) {
            _screenPos = Input.GetTouch(0).position;
        }
        else {
            return;
        }

        _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);
        
        if (_isDragActive) {
            Drag();
        }
        else {
            Debug.Log("ah oui");
            if (Physics.Raycast(_worldPos, _grabRay, out _hit, Mathf.Infinity)) {
                Debug.Log("Raycast Hit");
                Draggable3D draggable = _hit.transform.gameObject.GetComponent<Draggable3D>();
                if (draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag();
                }
                else Debug.Log("Object not draggable");
            }

        }
    }

    void InitDrag() {
        _isDragActive = true;
        _draggableJoint = _lastDragged.GetComponent<ConfigurableJoint>();
        _draggableJoint.anchor = _lastDragged.transform.InverseTransformPoint(new Vector3(_worldPos.x,_worldPos.y,0));
        _draggableJoint.connectedAnchor = _draggableJoint.anchor;
        _draggableJoint.xDrive = _attachedDrive;
        _draggableJoint.yDrive = _attachedDrive;
        _draggableJoint.angularYZDrive = _attachedAngularDrive;
        Time.timeScale = 0.5f;
    }

    void Drag()
    {
        _dragTarget.x = _worldPos.x;
        _dragTarget.y = _worldPos.y;
        _draggableJoint.connectedAnchor = _dragTarget;
    }

    void Drop() {
        _isDragActive = false;
        _draggableJoint.xDrive = _freeDrive;
        _draggableJoint.yDrive = _freeDrive;
        _draggableJoint.angularYZDrive = _freeDrive;
        Time.timeScale = 1f;

    }
}
