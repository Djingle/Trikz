using UnityEngine;

public class BottlePreviewManager : MonoBehaviour
{
    public static BottlePreviewManager Instance { get; private set; }
    [field: SerializeField] BottleData CurrentBottleData { get; set; }

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
        GameManager.StateChanged += (state) => SetPreviewActive(state == GameState.BottleMenu);
        MenuEvents.BottleSelectionChanged += UpdatePreview;
        SaveEvents.StateLoaded += (save) => UpdatePreview(save.EquippedBottle);
    }
    private void OnDisable()
    {
        GameManager.StateChanged -= (state) => SetPreviewActive(state == GameState.BottleMenu);
        MenuEvents.BottleSelectionChanged -= UpdatePreview;
        SaveEvents.StateLoaded -= (save) => UpdatePreview(save.EquippedBottle);
    }

    public void InitPreview()
    {
        _bottlePreviewObject = Instantiate(_bottlePreviewPrefab, new Vector3(100, 100, -100), Quaternion.identity);
        _bottlePreviewScript = _bottlePreviewObject.GetComponent<BottlePreview>();

        _previewCameraObject = Instantiate(_previewCameraPrefab, _bottlePreviewObject.transform.position + k_RelativeCamPos, Quaternion.identity);
        SetPreviewActive(false);
    }

    public void UpdatePreview(BottleData newBottleData)
    {
        if (_bottlePreviewObject == null) {
            InitPreview();
        }
        if (newBottleData) CurrentBottleData = newBottleData;
        _bottlePreviewScript.Init(CurrentBottleData);
    }

    public void SetPreviewActive(bool active)
    {
        Debug.Log("Preview : " + active);
        _bottlePreviewObject.SetActive(active);
        _previewCameraObject.SetActive(active);
    }
}
