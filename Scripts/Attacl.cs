using System.Runtime.CompilerServices;
using UnityEngine;

public class test2 : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] private float predkosc;
    [SerializeField] private float silaSkoku;
    [SerializeField] private float oporskoku;
    float lewoprawo;
    bool grounded;
    private Animator anim;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lewoprawo = Input.GetAxisRaw("Horizontal");

        if(lewoprawo != 0)
        {
            body.linearVelocity = new Vector2(lewoprawo * predkosc, body.linearVelocity.y); //chodzenie
        }


        if (Input.GetKey(KeyCode.Space) && grounded) //funkcja skoku
        {
           Jump();
        }
                    anim.SetBool("grounded",grounded); //nie wiem co to

        if (lewoprawo != 0)
        {
            if (lewoprawo > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0); // obrót postaci
            else
                transform.rotation = Quaternion.Euler(0, 180, 0); // obrót postaci

                anim.SetBool("Run", true); //Animacja biegu                                                                 //movement
        }
        else
            {
                anim.SetBool("Run", false); //
            }

        if (body.linearVelocity.y < 0) // Opadanie
        {
                body.AddForce(Vector2.up * oporskoku);      // Dodaje opór podczas skakania
        }



        if(Input.GetKey("mouse 0")) //atak
         {
            anim.SetBool("IsAttacking", true);                                                      //attak
         }



    }
private void Jump() //funkcja skoku
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, silaSkoku);
        grounded = false;
    }    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        grounded = true;
    }



    private void endAttack() //funkcja niekończąca animacji ataku gdy ten trwa
    {
        anim.SetBool("IsAttacking", false);
    }
    }

    