using UnityEngine;
using UnityEngine.SceneManagement;

// Toda esta clase ha sido creada porque intentar asignar la dificultad directamente a MapGenerator y que esta
// exista desde el menú principal es un infierno. Tampoco tiene sentido que algo como el generador esté en un
// menú principal de todas formas. Si me las ingenio de otra forma, se eliminará, pero por ahora es eficiente.
public class DifficultySettings : MonoBehaviour
{
    public static DifficultySettings Instance { get; private set; }

    public int difficulty { get; private set; }

    // Esto si se puede quedar para siempre, es configuración
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Reasigna el valor DE ESTA CLASE y llama a cargar la escena del juego
    public void StartNewGame(int difficultyValue)
    {
        difficulty = difficultyValue;
        SceneManager.LoadScene("GameScene");
    }
}
