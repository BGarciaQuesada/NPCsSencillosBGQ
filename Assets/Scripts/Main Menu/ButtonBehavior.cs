using UnityEngine;
using UnityEngine.SceneManagement;

// Esta clase almacena el valor de dificultad del boton y especifica su método al hacer clic.
public class ButtonBehavior : MonoBehaviour
{
    [Header("Valor de Dificultad")]
    [SerializeField] private int difficultyValue;

    public void LoadMap()
    {
        GameManager.Instance.StartNewGame(difficultyValue);
    }
}