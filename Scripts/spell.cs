using UnityEngine;

public class Spell : MonoBehaviour
{
    private float damage;

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            SamuraiHealth samuraiHealth = collision.GetComponent<SamuraiHealth>();
            if (samuraiHealth != null)
            {
                samuraiHealth.TakeDamage(damage);
            }
        }

        if (collision.CompareTag("Player2"))
        {
            ArcherHealth archerHealth = collision.GetComponent<ArcherHealth>();
            if (archerHealth != null)
            {
                archerHealth.TakeDamage(damage);
            }
        }
    }
}
