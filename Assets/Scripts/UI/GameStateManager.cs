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

    void Start()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        SetCursorLocked(true);
    }

    // ---------- FINALES ----------

    // [!] Esto se tiene que poderse combinar de alguna forma pero bueno, por ahora se queda así
    public void WinGame()
    {
        Debug.Log("Victoria");

        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Victory;
        endGameUI.ShowVictory();
        SetCursorLocked(false);
        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        Debug.Log("Derrota");

        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Defeat;
        endGameUI.ShowDefeat();
        SetCursorLocked(false);
        Time.timeScale = 0f;
    }

    // ---------- BOTÓN UI ----------

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // ------- MANEJO DE RATÓN -------
    void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

}
