using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private float lifetime;

   
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "enemies"){
            collision.GetComponent<EnemyHealth>()?.TakeDamage(5);
        }
        Destroy(gameObject);
    }
    
}
