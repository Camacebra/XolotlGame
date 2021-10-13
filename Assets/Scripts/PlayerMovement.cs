using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool IsGrounded { get; set; }
    [SerializeField] private float speed = 10f, 
                                   jumpForce = 200f;
    [Range(0, 0.3f)] [SerializeField] private float movementSmoothing = 0.03f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]private Transform[] groundChecks;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
        if (Input.GetKeyDown(KeyCode.W) && CheckIfGrounded())
            Jump();
    }

    private bool CheckIfGrounded()
    {
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer))
                return true;

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
        Vector3 targetVelocity = new Vector2(direction.x * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
    }
}
