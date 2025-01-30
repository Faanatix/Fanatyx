using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]  public int damage = 5; // Obrażenia zadawane przeciwnikowi

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Zadaj obrażenia przeciwnikowi
            Destroy(gameObject); // Zniszcz pocisk po trafieniu
        }

        // Opcjonalnie usuń pocisk przy kolizji z innymi obiektami
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}