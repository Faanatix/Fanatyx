using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 20; 
    private Animator anim; 
    private bool isDead = false; 

      void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Przeciwnik otrzymał obrażenia: " + damage);

        anim.SetTrigger("HitTrigger");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Przeciwnik zginął!");
        
        anim.SetTrigger("IsDead");
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }
}

    }
