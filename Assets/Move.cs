using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    public int endurance;
    public int maxEndurance;
    public GameObject playerInventoryUI;
    private Rigidbody2D playerRigidbody2D;
    private Animator animator;

    private float enduranceTimer = 0f;

    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("left_move", false);
        animator.SetBool("right_move", false);
    }

    void Update()
    {
        HandleEndurance();
        if (!playerInventoryUI.activeSelf)
        {
            PlayerMovement();
        }
    }

    private void HandleEndurance()
    {
        enduranceTimer += Time.deltaTime;

        float horizontalMovement, verticalMovement;
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        if ((horizontalMovement != 0 || verticalMovement != 0) && Input.GetKey(KeyCode.LeftShift) && endurance > 0)
        {
            if (enduranceTimer >= 0.5f)
            {
                endurance -= 1;
                endurance = Mathf.Max(endurance, 0);
                enduranceTimer = 0f;
            }
        }
        else
        {
            if (endurance < maxEndurance && enduranceTimer >= 1f)
            {
                endurance += 1;
                endurance = Mathf.Min(endurance, maxEndurance);
                enduranceTimer = 0f;
            }
        }
    }

    void PlayerMovement()
    {
        float horizontalMovement, verticalMovement;
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) && endurance > 0)
        {
            currentSpeed *= 1.5f;
        }

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            Vector2 movement = new Vector2(horizontalMovement, verticalMovement).normalized;
            playerRigidbody2D.velocity = movement * currentSpeed;
            if (movement.x <= 0)
            {
                animator.SetBool("left_move", true);
                animator.SetBool("right_move", false);
            }
            else if (movement.x > 0)
            {
                animator.SetBool("left_move", false);
                animator.SetBool("right_move", true);
            }
        }
        else
        {
            playerRigidbody2D.velocity = Vector2.zero;
            animator.SetBool("left_move", false);
            animator.SetBool("right_move", false);
        }
    }
}

