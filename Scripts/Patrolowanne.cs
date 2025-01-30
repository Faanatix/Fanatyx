using UnityEngine;

public class BOSS_AI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float spellRange = 15f; // Zasięg, w którym boss zaczyna rzucać zaklęcia
    [SerializeField] private float spellCooldown = 10f; // Czas między rzucaniem zaklęć
    [SerializeField] private float spellCastingTime = 2f; // Czas, w którym boss stoi nieruchomo

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("Spell Settings")]
    [SerializeField] private GameObject spellPrefab; // Prefab zaklęcia
    [SerializeField] private float spellYOffset = 1f; // Wysokość zaklęcia względem gracza
    [SerializeField] private float spellDamage = 2f; // Obrażenia zadawane przez zaklęcie
    [SerializeField] private float spellDuration = 5f; // Czas trwania zaklęcia na scenie

    private GameObject player1;
    private GameObject player2;
    private bool isDead;
    private bool isCasting; // Czy boss aktualnie rzuca zaklęcie
    private float lastSpellTime;

    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1"); // Łucznik
        player2 = GameObject.FindGameObjectWithTag("Player2"); // Samuraj
    }

    private void Update()
    {
        if (isDead || isCasting) return;

        float distanceToPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        // Jeśli gracz znajduje się w zasięgu zaklęcia
        if (distanceToPlayer1 <= spellRange || distanceToPlayer2 <= spellRange)
        {
            if (Time.time - lastSpellTime >= spellCooldown)
            {
                StartCoroutine(CastSpell()); // Rzucanie zaklęcia
                return;
            }
        }

        // Poruszanie się w kierunku gracza
        PatrolOrChase();
    }

    private void PatrolOrChase()
    {
        anim.SetBool("moving", true);

        // Poruszanie się w kierunku najbliższego gracza
        GameObject targetPlayer = GetClosestPlayer();
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        enemy.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1); // Obrót w stronę gracza
        transform.position += new Vector3(direction.x, 0, 0) * speed * Time.deltaTime;
    }

    private GameObject GetClosestPlayer()
    {
        float distanceToPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceToPlayer1 <= distanceToPlayer2) return player1;
        return player2;
    }

    private System.Collections.IEnumerator CastSpell()
    {
        isCasting = true;
        anim.SetBool("moving", false); // Zatrzymanie ruchu
        anim.SetTrigger("SpellTrigger"); // Aktywacja animacji zaklęcia

        yield return new WaitForSeconds(spellCastingTime); // Czas rzucania zaklęcia

        GameObject targetPlayer = GetClosestPlayer();
        if (targetPlayer != null)
        {
            // Tworzenie zaklęcia lekko wyżej nad graczem
            Vector3 spellPosition = targetPlayer.transform.position + new Vector3(0, spellYOffset, 0);
            GameObject spell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);

            // Przekazanie obrażeń zaklęciu
            Spell spellComponent = spell.GetComponent<Spell>();
            if (spellComponent != null)
            {
                spellComponent.SetDamage(spellDamage);
            }

            Destroy(spell, spellDuration); // Zaklęcie znika po określonym czasie
        }

        lastSpellTime = Time.time;
        isCasting = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Death");
        anim.SetBool("moving", false);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
