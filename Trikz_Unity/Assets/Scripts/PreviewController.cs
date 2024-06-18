using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewController : MonoBehaviour
{
    private Quaternion _minRot = Quaternion.Euler(0F, 0F, 0F);
    private Quaternion _maxRot = Quaternion.Euler(0F, 0F, 80F);
    private bool _goingUp;

    private void OnEnable()
    {
        LaunchScreenScript.HeightChanged += RotatePreview;
        RotatePreview(0f);
    }

    private void OnDisable()
    {
        LaunchScreenScript.HeightChanged -= RotatePreview;
    }

    public void RotatePreview(float time)
    {
        transform.rotation = Quaternion.Lerp(_minRot, _maxRot, time);
    }
}
