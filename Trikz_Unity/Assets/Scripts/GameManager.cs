using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }

    [field: SerializeField] public BottleData EquippedBottle { get; private set; }

    // GameState Event
    public static event Action<GameState> StateChanged;

    // Scores
    public int TotalPoints { get; set; } = 0;
    private int _pot = 1;
    private int _multipliedPot = 1;
    private int _highScore = 0;
    private float _multiplier = 1f;

    public GameObject _launchablePrefab;
    public GameObject _scorePanelPrefab;
    public GameObject _bottlePreviewPrefab;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

        MenuEvents.BottleEquipped += (v) => EquippedBottle = v;
        MenuEvents.BottleBought += BuyBottle;
        SaveEvents.StateLoaded += OnLoad;
    }

    private void OnDisable()
    {
        LaunchEvents.HasStopped -= OnLaunchableHasStopped;
        LaunchEvents.HasFlipped -= OnLaunchableHasFlipped;
        LaunchEvents.HeightDecided -= OnHeightDecided;
        LaunchEvents.StrengthDecided -= OnRotateStrengthDecided;

        MenuEvents.BottleEquipped -= (v) => EquippedBottle = v;
        MenuEvents.BottleBought += BuyBottle;
        SaveEvents.StateLoaded -= OnLoad;
    }

    private void Start()
    {
        // Load Save
        SaveManager.Instance.Load();
        // Go to menu state
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log(newState);
        State = newState;

        switch (newState) {
            case GameState.Launching:
                if (!Launchable.Instance) InitLaunchable();
                break;
            case GameState.Rotating:
                break;
            case GameState.Damping:
                break;
            case GameState.Menu:
                if (WspacePanel.Instance) Destroy(WspacePanel.Instance.gameObject);
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
        StateChanged?.Invoke(State);
    }

    public void InitLaunchable()
    {
        Instantiate(_launchablePrefab, Vector3.zero, Quaternion.identity);
        Launchable.Instance.Init(EquippedBottle);
        Instantiate(_scorePanelPrefab);
    }

    public void InitScores()
    {
        _multiplier = 1;
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier);
        ScoreEvents.TotalPointsChanged?.Invoke(TotalPoints);
    }

    public void DestroyLaunchable()
    {
        if (Launchable.Instance) Destroy(Launchable.Instance);
        else Debug.Log("No Launchable instance");
    }

    private void OnLaunchableHasStopped(bool win)
    {
        if (win) {
            _pot = _multipliedPot;
            ScoreEvents.CurrentPotChanged?.Invoke(_pot);
            ChangeState(GameState.Win);
        }
        else {
            _pot = 1;
            ScoreEvents.CurrentPotChanged?.Invoke(_pot);
            ChangeState(GameState.Lose);
        }
        Launchable.Instance.Reset();
    }


    private void OnLaunchableHasFlipped()
    {
        _multiplier = (_multiplier + 1);
        _multipliedPot = (int)MathF.Ceiling(_pot * _multiplier * EquippedBottle.Multiplier);

        ScoreEvents.CurrentPotChanged?.Invoke(_multipliedPot);
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier * EquippedBottle.Multiplier);
    }

    public void Withdraw()
    {

        TotalPoints += _pot;
        if (_pot > _highScore)
        {
            _highScore = _pot;
            ScoreEvents.HighScoreChanged?.Invoke(_highScore);
        }
        _pot = 1;
        _multiplier = 1;
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier * EquippedBottle.Multiplier);
        ScoreEvents.TotalPointsChanged?.Invoke(TotalPoints);
        ScoreEvents.CurrentPotChanged?.Invoke(_pot);
        ChangeState(GameState.Launching);
    }

    public void Replay()
    {
        _pot = 1;
        _multiplier = 1;
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier * EquippedBottle.Multiplier);
        ScoreEvents.CurrentPotChanged.Invoke(_pot);
        ChangeState(GameState.Launching);
    }

    public void Continue()
    {
        _multiplier = 1;
        ScoreEvents.MultiplierChanged?.Invoke(_multiplier);
        ChangeState(GameState.Launching);
    }

    private void BuyBottle(BottleData bottle)
    {
        TotalPoints -= bottle.Price;
        ScoreEvents.TotalPointsChanged?.Invoke(TotalPoints);
        SaveManager.Instance.State.BuyBottle(bottle);
        SaveManager.Instance.Save();
    }

    private void OnLoad(SaveState save)
    {
        // Load points 
        TotalPoints = save.Points;
        // Load the equipped bottle from save file
        BottleData saveBottle = save.EquippedBottle;
        if (saveBottle != null) EquippedBottle = saveBottle;
        // Tell everyone a new bottle is equipped
        MenuEvents.BottleEquipped?.Invoke(EquippedBottle);
        // Init scores
        InitScores();
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
    Menu,
    BottleMenu,
    OptionsMenu,
}