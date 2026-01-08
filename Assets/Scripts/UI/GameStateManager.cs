using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Victory,
    Defeat
}


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [SerializeField] private EndGameUI endGameUI;

    public GameState CurrentState { get; private set; } = GameState.Playing;

    // Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    // ---------- FINALES ----------

    public void WinGame()
    {
        Debug.Log("Victoria");

        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Victory;
        endGameUI.ShowVictory();
        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        Debug.Log("Derrota");

        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Defeat;
        endGameUI.ShowDefeat();
        Time.timeScale = 0f;
    }

    // ---------- BOTÓN UI ----------

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
