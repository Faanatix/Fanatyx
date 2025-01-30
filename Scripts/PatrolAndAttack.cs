using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy Settings")]
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float idleDuration = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackHitboxDelay = 0.5f;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    private Vector3 initScale;
    private bool movingLeft;
    private float idleTimer;
    private float lastAttackTime;
    private GameObject player1;
    private GameObject player2;
    private bool isDead;

    private void Awake()
    {
        initScale = enemy.localScale;
        player1 = GameObject.FindGameObjectWithTag("Player1");  // Łucznik
        player2 = GameObject.FindGameObjectWithTag("Player2");  // Samuraj
    }

private void Update()
{
    if (isDead) return;

    // Sprawdzamy tylko odległość w przestrzeni 2D (ignorujemy oś Z)
    float distanceToPlayer1 = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(player1.transform.position.x, player1.transform.position.y, 0));
    float distanceToPlayer2 = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(player2.transform.position.x, player2.transform.position.y, 0));

    Debug.Log("Distance to Player1 (Samurai): " + distanceToPlayer1);
    Debug.Log("Distance to Player2 (Archer): " + distanceToPlayer2);

    if (distanceToPlayer1 <= attackRange || distanceToPlayer2 <= attackRange)
    {
        AttackPlayer(distanceToPlayer1, distanceToPlayer2);
    }
    else
    {
        Patrol();
    }
}


    private void Patrol()
    {
        if (movingLeft)
        {
            if (Mathf.Abs(enemy.position.x - leftEdge.position.x) > 0.2f)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (Mathf.Abs(enemy.position.x - rightEdge.position.x) > 0.2f)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0f;
        }
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetBool("moving", true);  // Ustaw animację ruchu na true

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }

private void AttackPlayer(float distanceToPlayer1, float distanceToPlayer2)
{
    anim.SetBool("moving", false);

    if (Time.time - lastAttackTime >= attackCooldown)
    {
        anim.SetTrigger("isAttacking");
        lastAttackTime = Time.time;

        if (distanceToPlayer1 <= attackRange)
        {
            FlipTowards(player1.transform.position);
            Invoke(nameof(ActivateHitbox), attackHitboxDelay);
        }
        if (distanceToPlayer2 <= attackRange)
        {
            FlipTowards(player2.transform.position);
            Invoke(nameof(ActivateHitbox), attackHitboxDelay);
        }
    }
}

private void ActivateHitbox()
{
    if (isDead) return;

    if (Vector2.Distance(transform.position, player1.transform.position) <= attackRange)
    {
        SamuraiHealth samuraiHealth = player1.GetComponent<SamuraiHealth>();
        if (samuraiHealth != null)
        {
            samuraiHealth.TakeDamage(attackDamage);
        }
    }

    if (Vector2.Distance(transform.position, player2.transform.position) <= attackRange)
    {
        ArcherHealth archerHealth = player2.GetComponent<ArcherHealth>();
        if (archerHealth != null)
        {
            archerHealth.TakeDamage(attackDamage);
        }
    }
}

    private void FlipTowards(Vector3 target)
    {
        Vector3 scale = enemy.localScale;
        if (target.x > enemy.position.x)
        {
            scale.x = Mathf.Abs(scale.x);  // W prawo
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);  // W lewo
        }
        enemy.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        isDead = true;
        anim.SetTrigger("Death");
        anim.SetBool("moving", false);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; 
    }
}