using UnityEngine;
using UnityEngine.UI;

public class HB : MonoBehaviour
{
    [SerializeField] private SamuraiHealth playerHealth;
    [SerializeField] private Image HBTotal;
    [SerializeField] private Image HBCurrent;

    private void Start()
    {
        if (playerHealth != null)
        {
            HBTotal.fillAmount = playerHealth.currentHealth / 10f;
        }
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            HBCurrent.fillAmount = playerHealth.currentHealth / 10f;
        }
    }
}
