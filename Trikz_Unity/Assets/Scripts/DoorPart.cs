using UnityEngine;
using System.Collections;

public class DoorPart : MonoBehaviour
{
    public float _maxY;
    public float _minY;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setY(float y)
    {
        transform.position = new Vector3(transform.position.x, y);
    }

    public void setMinMaxY(float minY, float maxY)
    {
        _maxY = maxY;
        _minY = minY;
    }
}