using UnityEngine;

public class DoorController : MonoBehaviour
{
    public static DoorController instance;
    private DoorPart _lowPart, _highPart;

    [SerializeField] private float _yHoleOffset = 10f;

    [SerializeField] private float _minHoleSize = 2.5f;
    [SerializeField] private float _maxHoleSize = 6f;

    [SerializeField] private float _lowPartMinY = 0f;
    [SerializeField] private float _lowPartMaxY = 11f;

    [SerializeField] private float _highPartMinY = 11f;
    [SerializeField] private float _highParMaxY = 20f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GameManager.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        DoorPart[] doorParts = FindObjectsOfType<DoorPart>();
        if (doorParts.Length != 2)
        {
            Debug.Log("More or less than 2 doorParts");
            return;
        }
        _lowPart = doorParts[0];
        _highPart = doorParts[1];
        _lowPart.setMinMaxY(_lowPartMinY, _lowPartMaxY);
        _highPart.setMinMaxY(_highPartMinY, _highParMaxY);
        _lowPart.setY(_lowPartMinY);
        _highPart.setY(_highParMaxY);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void RandomizeDoor()
    {
        float holeSize = Random.Range(_minHoleSize, _maxHoleSize);
        float newHighY = Random.Range(_highPartMinY, _highParMaxY);
        float newLowY = newHighY - _yHoleOffset - holeSize;

        _lowPart.setY(newLowY);
        _highPart.setY(newHighY);
    }

    private void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Win || newState == GameState.Lose) RandomizeDoor();
    } 
}
