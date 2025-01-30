using UnityEngine;

public class Healthl : MonoBehaviour
{
    [SerializeField] private float startinghealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Gameover gameOverManager;

    [System.Obsolete]
    void Start()
    {
        currentHealth = startinghealth;
        anim = GetComponent<Animator>();
        gameOverManager = FindObjectOfType<Gameover>(); // Automatycznie znajdzie menedżera w scenie.
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startinghealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("injure");
        }
        else
        {
            anim.SetTrigger("Death");
            GetComponent<Collider2D>().enabled = false;

            PlayerControllerr test2 = GetComponent<PlayerControllerr>();
            if (test2 != null)
            {
                test2.enabled = false;
            }
            

            // Wywołaj ekran Game Over
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverScreen();
            }
        }
    }
}