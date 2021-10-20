using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    [SerializeField] private Transform[] raycastPos;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    private const int LAYER_OBJECTS = 11;
    private const int LAYER_GROUND = 10;
    private PlayerActions player;
    public bool IsBeingHeld { get; private set; }
    public bool IsFalling { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsFalling || IsBeingHeld)
        {
            if (collision.collider.name == "Player")
            {
                player.ChangeAction(2);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!player.HasItem  && !IsFalling || IsBeingHeld)
        {
            if (collision.collider.name == "Player")
            {
                player.ChangeAction(1);
            }
            HidePrompt();
        }
    }

    public void Grab(Transform player)
    {
        IsBeingHeld = true;
        transform.parent = player;
        transform.position = player.position;
        gameObject.layer = LAYER_OBJECTS;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.isKinematic = true;
        HidePrompt();
    }
    public void HidePrompt()
    {
        prompt.SetActive(false);
    }

    public void ShowPrompt()
    {
        prompt.SetActive(true);
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBeingHeld)
        {
            if (!CheckForGround())
            {
                Drop();
            }
        }
        if (IsFalling)
        {
            if (CheckForGround())
            {
                StopFall();
            }
        }
    }

    private void StopFall()
    {
        IsFalling = false;
    }

    public void Release()
    {
        transform.parent = null;
        gameObject.layer = LAYER_GROUND;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        IsBeingHeld = false;
    }

    private void Drop()
    {
        player.DropItem();
        transform.parent = null;
        gameObject.layer = LAYER_GROUND;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        IsBeingHeld = false;
        IsFalling = true;
        HidePrompt();
    }

    private bool CheckForGround()
    {
        RaycastHit2D hit;
        foreach (Transform pos in raycastPos)
        {
            hit = Physics2D.Raycast(pos.position, Vector2.down, .5f, groundLayer);
            if (hit)
            {
                return true;
            }
        }
        return false;
    }
}
