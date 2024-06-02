using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool _isLaunchableMoving = false;
    private Draggable3D _launchable;
    private TargetController _tc;
    private Cameraman _cameraman;
    private float _restCheckTimer = .5f;

    private void Awake()
    {
        GameController[] controllers = FindObjectsOfType<GameController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
        _tc = GameObject.FindObjectOfType<TargetController>();
        _launchable = GameObject.FindObjectOfType<Draggable3D>();
        _cameraman = GameObject.FindObjectOfType<Cameraman>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLaunchableMoving)
        {
            _restCheckTimer -= Time.deltaTime;
            if (_restCheckTimer <= 0)
            {
                if (!_launchable.IsMoving())
                {
                    _isLaunchableMoving = false;
                    CheckWin();
                }
                else _restCheckTimer = .5f;
            }
        }
    }

    public void NewLaunch()
    {
        _launchable.Reset();
        _tc.NewTarget();
        _cameraman.FocusLaunchable();
    }

    public void CheckWin()
    {
        if (_launchable.IsUpRight())
        {
            if (_tc.CheckIn(_launchable.transform.position.x))
            {
                // WIN
                Debug.Log("C'est gagné bien joué voilà");
            }
        }
        else
        {
            Debug.Log("Nul à chier le man");
        }
        NewLaunch();
    }

    public void Grab()
    {
        Time.timeScale = 0.75f;
    }

    public void Launch()
    {
        _isLaunchableMoving = true;
        Time.timeScale = 1f;
        _cameraman.FocusScene();
    }
}
