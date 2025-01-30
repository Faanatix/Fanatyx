using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] private float predkosc; // Prędkość ruchu
    [SerializeField] private float silaSkoku; // Siła skoku
    [SerializeField] private float oporskoku; // Opór podczas opadania
    private float lewoprawo; // Ruch w osi poziomej
    private bool grounded; // Czy gracz jest na ziemi
    private Animator anim; // Referencja do Animatora

    [Header("Melee Attack")]
    public GameObject attackPoint; // Punkt, z którego mierzony jest zasięg ataku
    public float radius = 0.5f; // Promień zasięgu ataku
    public LayerMask meleeEnemies; // Warstwa przeciwników dla ataku wręcz
    public float attackDelay = 0.5f; // Opóźnienie w sekundach przed aktywacją hitboxa

[Header("Dash Settings")]
public GameObject dashHitbox; // Hitbox dasha
public float dashForce = 20f; // Siła dasha (impuls)
public float dashDuration = 0.5f; // Czas trwania efektu dasha
public float dashCooldown = 2f; // Cooldown po dashu
public float dashCastTime = 0.3f; // Czas castowania dasha
public float dashAnimTime = 0.5f; // Czas trwania animacji dasha
public LayerMask dashEnemies; // Warstwa przeciwników dla dasha
[SerializeField] private int dashDamage = 20; // Obrażenia zadawane przez dash

private bool canDash = true; // Czy można wykonać dash
private bool isDashing = false; // Czy postać aktualnie dashuje

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Ruch w lewo/prawo
        lewoprawo = Input.GetAxisRaw("Horizontal");

        if (lewoprawo != 0)
        {
            body.linearVelocity = new Vector2(lewoprawo * predkosc, body.linearVelocity.y); // Ustaw prędkość ruchu

            // Obracanie postaci
            if (lewoprawo > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);

            anim.SetBool("Run", true); // Animacja biegu
        }
        else
        {
            anim.SetBool("Run", false); // Brak animacji ruchu
        }

        // Skok
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }

        // Atak
        if (Input.GetMouseButtonDown(0)) // Lewy przycisk myszy
        {
            StartCoroutine(PerformAttackWithDelay()); // Uruchom Coroutine z opóźnieniem
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // Dash na przycisk Shift
        {
            StartCoroutine(Dash());
        }

        // Opór podczas opadania
        if (body.linearVelocity.y < 0)
        {
            body.AddForce(Vector2.up * oporskoku);
        }

        // Ustawianie parametru grounded w Animatorze
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, silaSkoku);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    // Coroutine dla ataku wręcz
private IEnumerator PerformAttackWithDelay()
{
    Debug.Log("Rozpoczęcie ataku z opóźnieniem.");
    anim.SetTrigger("AttackTrigger");
    anim.SetBool("IsAttacking", true);

    yield return new WaitForSeconds(attackDelay);

    Debug.Log("Aktywacja hitboxa po opóźnieniu.");
    ActivateHitbox(meleeEnemies, attackPoint.transform.position, radius, 10);
    Debug.Log("Zakończenie ataku.");
}   
   public void EndAttack()
    {
        anim.SetBool("IsAttacking", false);
    }


    // Coroutine dla dasha
    private IEnumerator Dash()
{
    if (!canDash) yield break;
    canDash = false;

    anim.SetTrigger("DashTrigger");
    dashHitbox.SetActive(true);

    float dashDirection = transform.rotation.y == 0 ? 1 : -1;
    float dashTimeElapsed = 0f;
    HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    while (dashTimeElapsed < dashAnimTime)
    {
        body.linearVelocity = new Vector2(dashDirection * dashForce, body.linearVelocity.y);

        Collider2D[] detectedEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, dashEnemies);

        foreach (Collider2D enemyCollider in detectedEnemies)
        {
            GameObject enemy = enemyCollider.gameObject;
            if (!hitEnemies.Contains(enemy))
            {
                hitEnemies.Add(enemy);
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(dashDamage);
            }
        }

        dashTimeElapsed += Time.deltaTime;
        yield return null;
    }

    body.linearVelocity = Vector2.zero;
    dashHitbox.SetActive(false);
    yield return new WaitForSeconds(dashCooldown);
    canDash = true;
}

  private void ActivateHitbox(LayerMask targetEnemies, Vector2 position, float range, int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, range, targetEnemies);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Trafiony przeciwnik: " + enemy.name);
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    // Wizualizacja hitboxów w edytorze
    private void OnDrawGizmosSelected()
    {
        if (dashHitbox != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(dashHitbox.transform.position, 1f);
        }
    }
}