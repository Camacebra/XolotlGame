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
    private Renderer rend;
    private int currentMode;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfGrounded())
            Jump();
        ChangeMode();
    }

    private void ChangeMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMode =  currentMode > 0 ? currentMode - 1 : currentMode;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentMode = currentMode < 3 ? currentMode + 1 : currentMode;

        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        switch (currentMode)
        {
            case 0:
                rend.material.color = Color.white;
                break;
            case 1:
                rend.material.color = Color.blue;
                break;
            case 2:
                rend.material.color = Color.red;
                break;
            case 3:
                rend.material.color = Color.yellow;
                break;
            default:
                break;
        }
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
