using UnityEngine;

public class SamuraiHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Gameover gameOverManager;

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        gameOverManager = FindObjectOfType<Gameover>(); // Znajdź menedżera GameOver
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("injure"); // Animacja rany
        }
        else
        {
            anim.SetTrigger("Death"); // Animacja śmierci
            GetComponent<Collider2D>().enabled = false; // Wyłącz collider

            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverScreen(); // Pokaż ekran GameOver
            }

            // Wyłącz skrypt sterujący postacią (samurajem)
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // Wyłącz kontrolowanie samurajem
            }
        }
    }
}
