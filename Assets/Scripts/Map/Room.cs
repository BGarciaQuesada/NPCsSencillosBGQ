using UnityEngine;

// Esta clase maneja solo la lógica de las salas INDIVIDUALMENTE
public class Room : MonoBehaviour
{
    public Vector2Int gridPosition;

    [Header("Tapones")]
    [SerializeField] private GameObject northBlock;
    [SerializeField] private GameObject southBlock;
    [SerializeField] private GameObject eastBlock;
    [SerializeField] private GameObject westBlock;

    public void SetSideActive(Vector2Int direction, bool active)
    {
        if (direction == Vector2Int.up)
            northBlock.SetActive(active);
        else if (direction == Vector2Int.down)
            southBlock.SetActive(active);
        else if (direction == Vector2Int.right)
            eastBlock.SetActive(active);
        else if (direction == Vector2Int.left)
            westBlock.SetActive(active);
    }
}
