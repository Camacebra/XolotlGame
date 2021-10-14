using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedWithItem = 5f,
                                   speed = 10f, 
                                   jumpForce = 200f;
    [Range(0, 0.3f)] [SerializeField] private float movementSmoothing = 0.03f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]private Transform[] groundChecks;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector3 velocity = Vector3.zero;
    private bool isFacingRight = true;
    private PlayerActions playerActions;


    void Start()
    {
        playerActions = GetComponent<PlayerActions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
        if (!playerActions.HasItem && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckIfGrounded())
            Jump();
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D hit;
        foreach (Transform groundCheck in groundChecks)
        {
            hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
            if (hit)
            {
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        Vector3 targetVelocity = new Vector2(direction.x * (playerActions.HasItem ? speedWithItem: speed), rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        if (!playerActions.HasItem)
        {
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
        
    }

    private void Flip() 
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
