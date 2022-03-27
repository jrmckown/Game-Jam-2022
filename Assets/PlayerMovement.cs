using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Camera cam;
    public GameObject coin1;
    public GameObject coin2;
    public GameObject coin3;
    public GameObject coin4;
    public Text text;

    public Animator animator;

    float horizontalMove = 0;

    public float runSpeed = 40f;

    bool jump = false;
    bool crouch = false;
    bool moved = false;
    bool showTime = true;
    bool firstTime = true;

    int coinCount = 0;
    float time;


    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
            time = Time.time;
            firstTime = false;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        cam.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, -1);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
            moved = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

    }

    private void FixedUpdate()
    {
       
        if (coinCount == 4)
        {
            showTime = false;
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
        HorizontalMove();
        if (Dead())
        {
            controller.transform.position = new Vector3(-8, 1);
            ResetCoins();
            moved = false;
        }

        if (showTime)
        {
            text.text = Convert.ToString(Time.time - time);
        }
    }

    private void HorizontalMove()
    {
        if (horizontalMove != 0)
        {
            moved = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            collision.gameObject.transform.position = new Vector3(0, -10);
            coinCount++;
        }
    }

    private void ResetCoins()
    {
        coin1.transform.position = new Vector3(-0.33f, 0.6f);
        coin2.transform.position = new Vector3(-8.2f, 3.5f);
        coin3.transform.position = new Vector3(17.4f, 2.6f);
        coin4.transform.position = new Vector3(23.15f, -0.6f);
        coinCount = 0;
    }

    private bool Dead()
    {
        Rigidbody2D player = controller.gameObject.GetComponent<Rigidbody2D>();
        if (player.IsSleeping() && moved)
        {
            return true;
        }
        else if (controller.transform.position.y < -5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    /// <summary>
    /// check if the player stops moving to replace isSleeping method
    /// </summary>
    private void Idle()
    {
        
    }
}
