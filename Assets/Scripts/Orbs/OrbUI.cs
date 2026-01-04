using UnityEngine;
using TMPro;

public class OrbUI : MonoBehaviour
{
    [SerializeField] private TMP_Text orbText;

    private void Update()
    {
        orbText.text = "Orbes: " + OrbManager.Instance.totalOrbsCollected.ToString() + "/" + OrbManager.Instance.totalOrbsInLevel;
    }
}
