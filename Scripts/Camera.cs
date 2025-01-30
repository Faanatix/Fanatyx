using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Kamera")]
    [SerializeField] private float speed; // Jeśli potrzebujesz płynności ruchu
    [SerializeField] private Transform Samurai; 
    [SerializeField] private Transform Łucznik;

    private Transform currentTarget; // Obecny cel, za którym podąża kamera

    private void Start()
    {
        // Na starcie możesz ustawić domyślny cel (np. Samurai)
        currentTarget = Samurai;
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            // Kamera podąża za obecnym celem
            transform.position = new Vector3(currentTarget.position.x, transform.position.y, -10f);
        }
    }

    // Funkcja zmieniająca cel kamery
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }
}