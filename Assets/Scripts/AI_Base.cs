using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base : MonoBehaviour
{
    public struct TAG{
        public const string TAG_GROUND = "Ground",
                            TAG_BLOCK = "Block",
                            TAG_WATER = "Water",
                            TAG_BREACK = "Breackeable";
    }
    private const float DISTANCE_BLOCK = 1.25f,
                        DISNTANCE_GROUND = 0.5f,
                        SPEED = 5;
    [SerializeField] private Transform[] PosRaycastsDowns;
    [SerializeField] private Transform BlockRaycastPos;
    private Rigidbody2D rg;
    private Vector2 velocity;
    protected RaycastHit2D hit;
    protected float direction;
    private bool isGround, isWater;

    private bool isMoving { get;  set; }

    private void Awake(){
        rg = GetComponent<Rigidbody2D>();
        direction = 1;
        StartCoroutine(CheckingRaycastDelay());
    }

    private void FixedUpdate(){
        if (isMoving)
        {
            velocity.x = isGround ? direction * SPEED : 0;
            velocity.y = rg.velocity.y;
            rg.velocity = velocity;
        }
        else
        {
            rg.velocity = Vector2.zero;
        }
       
    }
    public virtual void Raycasting(){
        hit =  Physics2D.Raycast(BlockRaycastPos.position, transform.right * direction, DISTANCE_BLOCK, 1 << LayerMask.NameToLayer(TAG.TAG_BLOCK));
        direction *= hit? -1:1;
        foreach(Transform RaycastPos in PosRaycastsDowns){
            isGround = Physics2D.Raycast(RaycastPos.position, Vector2.down, DISNTANCE_GROUND, 1 << LayerMask.NameToLayer(TAG.TAG_GROUND));
            if (isGround) break;
        }
    }
    public virtual void OnWaterEnter(){
        Destroy(gameObject);
    }
    public virtual void OnWaterExit(){
    }
    public virtual void OnBreackEnter(Collider2D col){

    }
    IEnumerator CheckingRaycastDelay(){
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (enabled){
            Raycasting();
            yield return wait;
        }
    }

    internal void MovementSwitch()
    {
        isMoving = !isMoving;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(BlockRaycastPos.position, BlockRaycastPos.position + transform.right * DISTANCE_BLOCK * direction);
        foreach (Transform RaycastPos in PosRaycastsDowns)
            Gizmos.DrawLine(RaycastPos.position, RaycastPos.position + transform.up * DISNTANCE_GROUND * -1);
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.CompareTag(TAG.TAG_WATER)){
            Debug.Log("WATEER");
            isWater = true;
            OnWaterEnter();
        }
        if (collision.collider.CompareTag(TAG.TAG_BREACK)){
            OnBreackEnter(collision.collider);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TAG.TAG_WATER)){
            isWater = false;
            OnWaterExit();
        }
        
    }
}
