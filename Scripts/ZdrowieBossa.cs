using UnityEngine;

public class Patrolddddowanie : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Spell Settings")]
    [SerializeField] private GameObject spellPrefab;  // Prefab zaklęcia
    [SerializeField] private float spellRange = 15f;  // Zasięg, w którym boss rzuca zaklęcia
    [SerializeField] private float spellCooldown = 5f;  // Czas oczekiwania na ponowne rzucenie zaklęcia
    [SerializeField] private float spellYOffset = 7f; // Wysokość zaklęcia względem gracza (7 jednostek wyżej)
    private float lastSpellTime;

    [Header("Target for spell")]
    [SerializeField] private Transform spellTarget;  // Obiekt, na który będzie padać zaklęcie (możesz ustawić w edytorze)

    private GameObject player1;
    private GameObject player2;

    private bool isCastingSpell = false;  // Flaga, aby zapobiec wielokrotnemu rzucaniu zaklęcia

    private void Awake()
    {
        initScale = enemy.localScale;
        player1 = GameObject.FindGameObjectWithTag("Player1");  // Łucznik
        player2 = GameObject.FindGameObjectWithTag("Player2");  // Samuraj
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (Time.time - lastSpellTime >= spellCooldown)
        {
            if (IsPlayerInRange() && !isCastingSpell)  // Sprawdzamy czy nie rzucamy zaklęcia
            {
                StartCoroutine(CastSpell());
                return;
            }
        }

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private bool IsPlayerInRange()
    {
        // Sprawdzenie, czy któryś z graczy jest w zasięgu, aby rzucić zaklęcie
        float distanceToPlayer1 = Vector3.Distance(enemy.position, player1.transform.position);
        float distanceToPlayer2 = Vector3.Distance(enemy.position, player2.transform.position);
        return distanceToPlayer1 <= spellRange || distanceToPlayer2 <= spellRange;
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        // Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }

    private System.Collections.IEnumerator CastSpell()
    {
        isCastingSpell = true;  // Ustawiamy flagę na true, aby zapobiec kolejnym rzutom zaklęcia

        anim.SetBool("moving", false); // Zatrzymanie ruchu
        anim.SetTrigger("SpellTrigger"); // Aktywowanie animacji rzucania zaklęcia

        yield return new WaitForSeconds(1f); // Czas rzucania zaklęcia, np. 1 sekunda

        // Jeśli mamy przypisany obiekt na który ma spaść zaklęcie
        if (spellTarget != null)
        {
            // Obliczamy pozycję zaklęcia - nad obiektem, na który ma spaść
            Vector3 spellPosition = spellTarget.position + new Vector3(0, spellYOffset, 0);  // Zaklęcie pojawi się nad obiektem
            Instantiate(spellPrefab, spellPosition, Quaternion.identity);  // Rzucenie zaklęcia
        }

        lastSpellTime = Time.time; // Zresetowanie czasu rzucania zaklęcia

        // Resetowanie flagi po zakończeniu rzucania zaklęcia
        isCastingSpell = false;
    }

   
}
