using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [Header("Valor de Dificultad")]
    [SerializeField] private int difficultyValue;

    public void LoadMap()
    {
        // Verifica si la instancia existe antes de modificar la dificultad
        if (MapGenerator.Instance != null)
        {
            MapGenerator.Instance.difficulty = difficultyValue;
            Debug.Log($"Dificultad establecida a: {difficultyValue}");
        }

        // Ahora carga la escena
        SceneManager.LoadScene("GameScene");
    }
}