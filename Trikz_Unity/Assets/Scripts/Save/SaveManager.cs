using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveState State { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Debug.LogWarning("SaveManager duplicate !");
            Destroy(this);
            return;
        } else {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        GameManager.StateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= OnGameStateChanged;
    }

    public void Save()
    {
        string saveString = State.Serialize();
        Debug.Log("Saving :\n" + saveString);
        PlayerPrefs.SetString("save", saveString);
        SaveEvents.StateSaved?.Invoke(State);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save")) {
            string loadString = PlayerPrefs.GetString("save");
            State = loadString.Deserialize<SaveState>();
            Debug.Log("Loading :\n" + loadString);
        }
        else {
            State = new SaveState();
            State.WriteState(GameManager.Instance.TotalPoints,
                             GameManager.Instance.EquippedBottle,
                             new List<BottleData> {GameManager.Instance.EquippedBottle});
            Debug.Log("No save file found, creating a new one !");
            Save();
        }
        SaveEvents.StateLoaded?.Invoke(State);
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState) {
            case GameState.Win:
            case GameState.Lose:
            case GameState.Menu:
                State.WriteState(GameManager.Instance.TotalPoints, GameManager.Instance.EquippedBottle, null);
                Save();
                break;
        }
    }
}
