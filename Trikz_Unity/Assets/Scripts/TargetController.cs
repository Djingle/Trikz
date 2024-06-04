using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private float _xUpLim;
    private float _xLowLim;
    private float _xStart;
    private float _xEnd;
    private float _minSize = 2f;
    private GameObject[] _walls;

    private void Awake()
    {
        _walls = GameObject.FindGameObjectsWithTag("LimitWall");
        if (_walls.Length > 2) {
            Debug.LogError("More than 2 walls");
        }

        // Assign low and upper lim
        if (_walls[0].transform.position.x > _walls[1].transform.position.x ) {
            _xUpLim = _walls[0].transform.position.x;
            _xLowLim = _walls[1].transform.position.x;
        }
        else {
            _xUpLim = _walls[1].transform.position.x;
            _xLowLim = _walls[0].transform.position.x;
        }
    }

    public void NewTarget()
    {
        _xStart = Random.Range(_xLowLim, _xUpLim - _minSize);
        _xEnd = Random.Range(_xStart + -_minSize, _xUpLim);
        transform.position = new Vector3((_xStart + _xEnd) / 2, 0.501f, 0f);
        transform.localScale = new Vector3(Random.Range(.2f, 1f), 1f, 1f);
    }

    void Update()
    {
        
    }

    public bool CheckIn(float x)
    {
        if (x < transform.position.x + transform.localScale.x * 5 &&
            x > transform.position.x - transform.localScale.x * 5)
            return true;
        else return false;
    }
}
