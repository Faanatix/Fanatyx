using UnityEngine;
using UnityEngine.UI;

public class ArcherHB : MonoBehaviour
{
    [SerializeField] private ArcherHealth playerHealth;
    [SerializeField] private Image HBTotal;
    [SerializeField] private Image HBCurrent;

    private void Start()
    {
        if (playerHealth != null)
        {
            HBTotal.fillAmount = playerHealth.currentHealth / 10f; // Zakładając, że health łucznika ma maksymalnie 10
        }
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            HBCurrent.fillAmount = playerHealth.currentHealth / 10f; // Aktualizuj pasek zdrowia
        }
    }
}
