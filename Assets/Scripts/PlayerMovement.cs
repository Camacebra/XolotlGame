using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private struct PlayerAudios
    {
        public const string JUMP = "jump",
                             BARK = "bark",
                             STEP = "step";
    }
    private const string ANIM_IDLE = "Idle",
                        ANIM_WALK = "Walk",
                        ANIM_JUMP = "Jump",
                        ANIM_JUMP_IDLE = "JumpIdle",
                        ANIM_SLEEP_IDLE = "SleepIdle",
                        ANIM_AWAKE = "Awake",
                        ANIM_BARK = "Bark";

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
    private int prevSound;
   
    public bool IsMoving { get; private set; }
    public bool IsJumping { get; private set; }
    public bool HasReachedPeak { get; private set; }
    public bool HasJumped { get; private set; }
    public bool IsBarking { get; private set; }
    public bool IsActive { get; private set; }

    void Start()
    {
        IsActive = false;
        prevSound = -1;
        IsMoving = false;
        IsJumping = false;
        IsBarking = false;
        animator = GetComponent<Animator>();
        playerActions = GetComponent<PlayerActions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (IsActive)
        {

            direction = Vector2.zero;
            if (!IsBarking)
            {
                direction = new Vector2(Input.GetAxis("Horizontal"), transform.position.y);
                if (!playerActions.HasItem && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckIfGrounded())
                    Jump();
                if (HasReachedPeak && CheckIfGrounded())
                {
                    IsJumping = false;
                    HasReachedPeak = false;
                }
            }
            SelectAnimation();
        }
    }


    public bool CheckIfGrounded()
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
        HasJumped = true;
        IsJumping = true;
        rb.AddForce(new Vector2(0f, jumpForce));
        if (Helpers.AudioManager.instance)
            Helpers.AudioManager.instance.PlayClip(PlayerAudios.JUMP);
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
        if (Helpers.AudioManager.instance && direction.x !=0 && CheckIfGrounded())
        {
            int RandomSound;
            do
                RandomSound = UnityEngine.Random.Range(1, 5);
            while (RandomSound == prevSound);
            prevSound = RandomSound;
            Helpers.AudioManager.instance.PlayClip(PlayerAudios.STEP + RandomSound.ToString(), UnityEngine.Random.Range(0.25f, 0.4f));
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
        if (playerActions.HasBarked)
        {
            playerActions.HasBarked = false;
            IsBarking = true;
            ChangeCurrentAnimation(ANIM_BARK);
            StartCoroutine(PlayNextAnimation("StopBark"));
        }
        if (HasJumped)
        {
            HasJumped = false;
            ChangeCurrentAnimation(ANIM_JUMP);
            StartCoroutine(PlayNextAnimation("PlayJumpIdle"));
        }
        else if (!IsJumping && !IsBarking && IsMoving)
        {
            ChangeCurrentAnimation(ANIM_WALK);
        }
        else if(!IsJumping && !IsBarking && !IsMoving)
        {
            ChangeCurrentAnimation(ANIM_IDLE);
        }
    }


    public void Awaken()
    {
        ChangeCurrentAnimation(ANIM_AWAKE);
        StartCoroutine(PlayNextAnimation("Activate"));
    }

    private void Activate()
    {
        IsActive = true;
        ChangeCurrentAnimation(ANIM_IDLE);
    }

    private void StopBark()
    {
        IsBarking = false;
    }

    private void PlayJumpIdle()
    {
        IsJumping = true;
        HasReachedPeak = true;
        ChangeCurrentAnimation(ANIM_JUMP_IDLE);
    }

    private void ChangeCurrentAnimation(string anim)
    {
        if (currentAnim == anim)
            return;
        animator.Play(anim);
        currentAnim = anim;
    }

    IEnumerator PlayNextAnimation(string method)
    {
        yield return new WaitForEndOfFrame();
        Invoke(method, animator.GetCurrentAnimatorStateInfo(0).length);
    }

}
