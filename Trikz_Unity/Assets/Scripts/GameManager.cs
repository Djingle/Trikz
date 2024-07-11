using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }

    [field: SerializeField] public BottleData SelectedBottleData { get; private set; }

    // GameState Event
    public static event Action<GameState> StateChanged;

    // Flags
    private bool _isOver = false;

    // Scores
    private int _totalPoints = 0;
    private int _currentPot = 1;
    private int _multipliedPot = 1;
    private int _highScore = 0;
    private float _multiplier = 1f;

    public GameObject _launchablePrefab;
    public GameObject _bottlePreviewPrefab;



    private void Awake()
    {
        if (Instance != null &&  Instance != this) {
            Destroy(this);
            return;
        }
        else {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        LaunchEvents.HasStopped += OnLaunchableHasStopped;
        LaunchEvents.HasFlipped += OnLaunchableHasFlipped;
        LaunchEvents.HeightDecided += OnHeightDecided;
        LaunchEvents.StrengthDecided += OnRotateStrengthDecided;

        MenuEvents.BottleEquipped += (v) => SelectedBottleData = v;
    }

    private void OnDisable()
    {
        LaunchEvents.HasStopped -= OnLaunchableHasStopped;
        LaunchEvents.HasFlipped -= OnLaunchableHasFlipped;
        LaunchEvents.HeightDecided -= OnHeightDecided;
        LaunchEvents.StrengthDecided -= OnRotateStrengthDecided;

        MenuEvents.BottleEquipped -= (v) => SelectedBottleData = v;
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
        BottlePreviewManager.Instance.UpdatePreview(SelectedBottleData);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;

        switch (newState) {
            case GameState.Launching:
                if (!Launchable.Instance) InitLaunchable(BottleType.Green);
                break;
            case GameState.Rotating:
                break;
            case GameState.Damping:
                break;
            case GameState.Menu:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }

        StateChanged?.Invoke(State);
    }

    public void InitLaunchable(BottleType type)
    {
        Instantiate(_launchablePrefab, Vector3.zero, Quaternion.identity);
        Launchable.Instance.Init(SelectedBottleData);
    }

    public void DestroyLaunchable()
    {
        if (Launchable.Instance) Destroy(Launchable.Instance);
        else Debug.Log("No Launchable instance");
    }

    private void OnLaunchableHasStopped(bool win)
    {
        if (win) {
            _currentPot = _multipliedPot;
            ScoreEvents.CurrentPotChanged?.Invoke(_currentPot);
            ChangeState(GameState.Win);
        }
        else {
            _currentPot = 1;
            ScoreEvents.CurrentPotChanged?.Invoke(_currentPot);
            ChangeState(GameState.Lose);
        }
        _isOver = true;
    }


    private void OnLaunchableHasFlipped()
    {
        _multiplier *= 1.5f;
        _multipliedPot = (int)MathF.Ceiling(_currentPot * _multiplier);

        ScoreEvents.CurrentPotChanged?.Invoke(_multipliedPot);
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier);
    }

    public void Withdraw()
    {
        if (!_isOver) return;

        _totalPoints += _currentPot;
        if (_currentPot > _highScore)
        {
            _highScore = _currentPot;
            ScoreEvents.HighScoreChanged?.Invoke(_highScore);
        }
        _currentPot = 1;

        ScoreEvents.TotalPointsChanged.Invoke(_totalPoints);
        ScoreEvents.CurrentPotChanged.Invoke(_currentPot);
        ChangeState(GameState.Launching);
    }

    public void Replay()
    {
        if (!_isOver) return;
        _currentPot = 1;
        _multiplier = 1;
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier);
        ScoreEvents.CurrentPotChanged.Invoke(_currentPot);
        ChangeState(GameState.Launching);
    }

    public void Continue()
    {
        ChangeState(GameState.Launching);
    }

    private void OnHeightDecided(float height)
    {
        ChangeState(GameState.Rotating);
    }

    private void OnRotateStrengthDecided(float rotateStrength)
    {
        ChangeState(GameState.Damping);
    }
}

public enum GameState
{
    Launching,
    Rotating,
    Damping,
    Win,
    Lose,
    Menu
}