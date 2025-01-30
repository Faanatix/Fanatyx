using UnityEngine;

public class Patrolssowanie : MonoBehaviour
{
    [Header ("Patrol Points")]
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
    [SerializeField] private float spellCooldown = 15f; // Czas, co ile rzucamy zaklęcie (15 sekund)
    private float lastSpellTime; // Czas ostatniego rzucenia zaklęcia

    [Header("Player Settings")]
    [SerializeField] private Transform playerTransform; // Transform gracza

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        // Sprawdzamy, czy gracz znajduje się w zasięgu rzucania zaklęcia
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Jeśli gracz jest w zasięgu (<= 15 jednostek) i minął czas oczekiwania na kolejne zaklęcie
        if (distanceToPlayer <= 15f && Time.time - lastSpellTime >= spellCooldown)
        {
            CastSpell(); // Rzucamy zaklęcie
        }

        // Ruch w lewo lub prawo
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

    private void CastSpell()
    {
        // Aktywacja animacji zaklęcia na grzechu
        anim.SetTrigger("CastSpell"); // Przypisz odpowiedni trigger animacji w Animatorze
        lastSpellTime = Time.time; // Aktualizowanie czasu ostatniego rzucenia zaklęcia
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
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
}
