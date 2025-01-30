using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class PlayerControllerr : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] private float predkosc; // Prędkość ruchu
    [SerializeField] private float silaSkoku; // Siła skoku
    [SerializeField] private float oporskoku; // Opór podczas opadania
    private float lewoprawo; // Ruch w osi poziomej
    private bool grounded; // Czy gracz jest na ziemi
    private Animator anim; // Referencja do Animatora

    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;

    public Transform LaunchOffset;






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
            anim.SetBool("Run", false); 
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
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown) 
        {
        
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            
        }

        // Opór podczas opadania
        if (body.linearVelocity.y < 0)
        {
            body.AddForce(Vector2.up * oporskoku);
        }

        // Ustawianie parametru grounded w Animatorze
        anim.SetBool("grounded", grounded);
         cooldownTimer += Time.deltaTime;
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
}