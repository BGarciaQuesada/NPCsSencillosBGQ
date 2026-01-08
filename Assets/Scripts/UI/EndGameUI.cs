using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    [Header("Sprites")]
    [SerializeField] private Sprite victorySprite;
    [SerializeField] private Sprite defeatSprite;

    //void Awake()
    //{
    //    Debug.Log("Comienza el juego, escondo el panel.");
    //    gameObject.SetActive(false);
    //}

    public void ShowVictory()
    {
        gameObject.SetActive(true);
        titleText.text = "VICTORIA";
        descriptionText.text = "Has recolectado todos los orbes y escapado.";
        iconImage.sprite = victorySprite;
    }

    public void ShowDefeat()
    {
        gameObject.SetActive(true);
        titleText.text = "DERROTA";
        descriptionText.text = "Has sido atrapado.";
        iconImage.sprite = defeatSprite;
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
