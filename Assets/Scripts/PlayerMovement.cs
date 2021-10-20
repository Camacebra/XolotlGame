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
    private Animator animator;
    private Vector3 velocity = Vector3.zero;
    private bool isFacingRight = true;
    private PlayerActions playerActions;
    private string currentAnim;
    private const string ANIM_IDLE = "Idle", ANIM_WALK = "Walk";
    public bool IsMoving { get; private set; }
    public bool IsJumping { get; private set; }

    void Start()
    {
        IsMoving = false;
        IsJumping = false;
        animator = GetComponent<Animator>();
        playerActions = GetComponent<PlayerActions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
        if (!playerActions.HasItem && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckIfGrounded())
            Jump();
        SelectAnimation();
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
        IsMoving = false;
        if (!playerActions.HasItem && direction.x != 0)
        {
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
            IsMoving = true;
        }
        
    }

    private void Flip() 
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void SelectAnimation()
    {
        if (IsMoving && !playerActions.HasItem && !IsJumping)
        {
            ChangeCurrentAnimation(ANIM_WALK);
        }
        else if(!IsMoving && !playerActions.HasItem && !IsJumping)
        {
            ChangeCurrentAnimation(ANIM_IDLE);
        }
    }

    private void ChangeCurrentAnimation(string anim)
    {
        if (currentAnim == anim)
            return;
        animator.Play(anim);
        currentAnim = anim;
    }

}
