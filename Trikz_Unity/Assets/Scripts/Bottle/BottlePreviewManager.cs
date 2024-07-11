using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlePreviewManager : MonoBehaviour
{
    public static BottlePreviewManager Instance { get; private set; }

    [SerializeField] GameObject _bottlePreviewPrefab;
    [SerializeField] GameObject _previewCameraPrefab;

    GameObject _bottlePreviewObject;
    BottlePreview _bottlePreviewScript;

    GameObject _previewCameraObject;

    public readonly Vector3 k_PreviewPos = new Vector3(100,100,100);
    public readonly Vector3 k_RelativeCamPos = new Vector3(0, 0, -3);
    
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        MenuEvents.BottleButtonClicked += () => SetPreviewActive(true);
        MenuEvents.BottleSelectionChanged += UpdatePreview;
    }
    private void OnDisable()
    {
        MenuEvents.BottleButtonClicked -= () => SetPreviewActive(true);
        MenuEvents.BottleSelectionChanged -= UpdatePreview;
    }

    public void InitPreview()
    {
        _bottlePreviewObject = Instantiate(_bottlePreviewPrefab, new Vector3(100, 100, -100), Quaternion.identity);
        _bottlePreviewScript = _bottlePreviewObject.GetComponent<BottlePreview>();

        _previewCameraObject = Instantiate(_previewCameraPrefab, _bottlePreviewObject.transform.position + k_RelativeCamPos, Quaternion.identity);
        SetPreviewActive(false);
    }

    public void UpdatePreview(BottleData bottleData)
    {
        if (_bottlePreviewObject == null) {
            InitPreview();
        }
        _bottlePreviewScript.Init(bottleData);
    }

    public void SetPreviewActive(bool active)
    {
        _bottlePreviewObject.SetActive(active);
        _previewCameraObject.SetActive(active);
    }
}
