using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{

    private void Start()
    {
        Debug.Log(Camera.main.projectionMatrix);
    }

    void FocusScene()
    {
        //_cam.projection
    }
}
