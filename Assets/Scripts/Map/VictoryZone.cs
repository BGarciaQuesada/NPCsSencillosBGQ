using UnityEngine;

// Este método se asignará a una sala (spawn por el momento) en la cual, al volver con todos los orbes, se ganará el juego
public class VictoryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (OrbManager.Instance.allCollected)
        {
            GameStateManager.Instance.WinGame();
        }
    }
}
