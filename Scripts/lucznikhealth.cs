using UnityEngine;

public class ArcherHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Gameover gameOverManager;

    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        gameOverManager = FindFirstObjectByType<Gameover>(); // Znajdź menedżera GameOver
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

            // Wyłącz skrypty sterujące postacią (łucznikiem)
            PlayerControllerr playerControllerr = GetComponent<PlayerControllerr>();
            PlayerShooting playerShooting = GetComponent<PlayerShooting>();

            if (playerControllerr != null)
            {
                playerControllerr.enabled = false; // Wyłącz kontrolowanie łucznikiem
            }

            if (playerShooting != null)
            {
                playerShooting.enabled = false; // Wyłącz strzelanie
            }
        }
    }
}
