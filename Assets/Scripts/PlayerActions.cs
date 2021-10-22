using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color white;
    [ColorUsage(true, true)]
    public Color red;
    [ColorUsage(true, true)]
    public Color blue;
    [ColorUsage(true, true)]
    public Color yellow;
    private SpriteRenderer rend;
    private Material myMat;
    [HideInInspector]public int currentMode = 5, currentActionType = 1, currentProgress;
    private const string TAG_INTERACTABLE = "Interactable";
    private Spawner spawn;
    [SerializeField] private Transform pickupPos;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private BoxCollider2D boxCollider;
    private GameObject objectInFront;
    private PlayerMovement move;
    private const float DISTANCE_OBJECT = 1.5F;
    private bool isMoving = false;
    public bool HasItem { get; private set; }
    public bool HasBarked { get; set; }
    public bool CanBark { get; set; }
    public bool CanChangeMode { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        currentProgress = 0;
        CanChangeMode = true;
        CanBark = false;
        currentMode = 5;
        rend = GetComponent<SpriteRenderer>();
        myMat = rend.material;
        move = GetComponent<PlayerMovement>();
        myMat.EnableKeyword("_Ecolor");
        spawn = FindObjectOfType<Spawner>();

    }
    private void OnEnable()
    {
        spawn = FindObjectOfType<Spawner>();
    }
    // Update is called once per frame
    void Update()
    {
        ChangeMode();
        if (move.IsActive && Input.GetKeyDown(KeyCode.Space))
        {
            DoAction();
        }
    }

    public void ChangeAction(int i)
    {
        if (!HasItem)
        {
            currentActionType = i;
            if (i == 1)
                objectInFront = null;
            else
            {
                GetObject();
                if (objectInFront == null)
                {
                    currentActionType = 1;
                }
            }

        }

    }

    private void GetObject()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(pickupPos.position, transform.right, DISTANCE_OBJECT, objectLayer);
        if (hit)
        {
            objectInFront = hit.collider.gameObject;
            if (objectInFront != null)
            {
                objectInFront.SendMessage("ShowPrompt");
            }
        }
    }

    private void DoAction()
    {
        switch (currentActionType)
        {
            case 1:
                if (CanBark && !move.IsJumping && move.CheckIfGrounded())
                {
                    spawn.CommandSouls(currentMode, transform.position, actionableRadius);
                    if (Helpers.AudioManager.instance)
                        Helpers.AudioManager.instance.PlayClip("bark" + (isMoving ? "1" : "2"));
                    isMoving = !isMoving;
                    HasBarked = true;
                }
                break;
            case 2:
                GrabItem();
                break;
            case 3:
                ReleaseItem();
                break;
            case 4:
                ActivateSwitch();
                break;
            default:
                break;
        }

    }

    private void ActivateSwitch()
    {
        objectInFront.SendMessage("Activate");
    }

    public void ReleaseItem()
    {
        if (objectInFront != null)
        {
            boxCollider.enabled = false;

            HasItem = false;
            currentActionType = 1;
            objectInFront.SendMessage("Release");
            objectInFront = null;
        }
    }

    public void DropItem()
    {
        boxCollider.enabled = false;
        HasItem = false;
        currentActionType = 1;
    }

    private void GrabItem()
    {
        if (objectInFront != null)
        {

            boxCollider.enabled = true;
            objectInFront.SendMessage("Grab", pickupPos);
            HasItem = true;
            currentActionType = 3;
        }
    }

    private void ChangeMode()
    {
        if (CanChangeMode && currentProgress > 1)
        {


            if (Input.GetKeyDown(KeyCode.Q))
            {
                Helpers.AudioManager.instance.PlayClip("xolo_switch");
                currentMode = currentMode > 0 ? currentMode - 1 : currentProgress - 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Helpers.AudioManager.instance.PlayClip("xolo_switch");

                currentMode = currentMode < currentProgress - 1 ? currentMode + 1 : 0;

            }
        }
    }

    public void IncreaseProgress()
    {
        currentProgress++;
        CanBark = true;
        Helpers.AudioManager.instance.PlayClip("xolo_switch");
        currentMode = currentProgress - 1;
    }

 

    [Range(0, 10f)] public float actionableRadius = 8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasItem && collision.tag == TAG_INTERACTABLE)
        {
            currentActionType = 4;
            objectInFront = collision.gameObject;
            objectInFront.SendMessage("EnablePrompt");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!HasItem && currentActionType == 4)
        {
            ChangeAction(1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, actionableRadius);
        Gizmos.DrawLine(pickupPos.position, pickupPos.position + transform.right * DISTANCE_OBJECT);
    }
}
