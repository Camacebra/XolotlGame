using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Renderer rend;
    private int currentMode, currentActionType = 1;
    private const string TAG_INTERACTABLE = "Interactable";
    private Spawner spawn;
    [SerializeField] private Transform pickupPos;
    private GameObject objectInFront;
    [SerializeField] private LayerMask objectLayer;
    private PlayerMovement move;
    private const float DISTANCE_OBJECT = 1.5F;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        spawn = GameObject.Find("Spawner").GetComponent<Spawner>();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeMode();
        if (Input.GetKeyDown(KeyCode.Space))
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
                GetObject();
        }

    }

    private void GetObject()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(pickupPos.position, transform.right, 1.5f, objectLayer);
        if (hit)
        {
            objectInFront = hit.collider.gameObject;
            objectInFront.SendMessage("ShowPrompt");
        }
    }

    private void DoAction()
    {
        switch (currentActionType)
        {
            case 1:
                spawn.CommandSouls(currentMode, transform.position, actionableRadius);
                break;
            case 2:
                GrabItem();
                break;
            case 3:
                ReleaseItem();
                break;
            default:
                break;
        }

    }

    public void ReleaseItem()
    {
        Debug.Log("here");
        HasItem = false;
        currentActionType = 1;
        objectInFront.SendMessage("Release");
        objectInFront = null;
    }

    public void DropItem()
    {
        Debug.Log("HERE2");
        HasItem = false;
        currentActionType = 1;
    }

    private void GrabItem()
    {
        objectInFront.SendMessage("Grab", pickupPos);
        HasItem = true;
        currentActionType = 3;
    }

    private void ChangeMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMode = currentMode > 0 ? currentMode - 1 : currentMode;
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

    [Range(0, 10f)] public float actionableRadius = 8f;

    public bool HasItem { get; private set; }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, actionableRadius);
        Gizmos.DrawLine(pickupPos.position, pickupPos.position + transform.right * DISTANCE_OBJECT);
    }
}
