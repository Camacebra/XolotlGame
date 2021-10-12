using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool IsGrounded { get; set; }
    [SerializeField] private float speed = 10f, jumpForce = 200f;
    [Range(0, 0.3f)] [SerializeField] private float movementSmoothing = 0.03f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]private Transform groundCheck;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfGrounded())
            Jump();

    }

    private bool CheckIfGrounded()
    {
        return Physics2D.Raycast(transform.position, groundCheck.position, 0.5f, groundLayer);
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
        Vector3 targetVelocity = new Vector2(direction.x * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
    }
}
