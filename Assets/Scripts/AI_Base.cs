using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base : MonoBehaviour
{
    public struct TAG{
        public const string TAG_GROUND = "Ground",
                            TAG_WATER = "Water",
                            TAG_BREACK = "Breackeable";
    }
    private const float DISTANCE_BLOCK = 1.25f,
                        DISNTANCE_GROUND = 0.25f,
                        SPEED = 5,
                        ///JUMP//
                        AIR_SPEED = 1,
                        JUMP_FORCE = 420,
                        MAXJUMP_DISTANCE = 2,
                        JUMP_TIME = 0.2f;
    [SerializeField] private Transform[] PosRaycastsDowns;
    [SerializeField] private Transform BlockRaycastPos;
    public LevelManager.TypeSoul myTypeSoul;
    private Rigidbody2D rg;
    private Vector2 velocity;
    protected RaycastHit2D hit;
    protected float direction, prevJumpTime;
    public bool isGround, isWater, isJumping;
    
    private bool isMoving { get;  set; }

    private void Awake(){
        rg = GetComponent<Rigidbody2D>();
        direction = 1;
        isJumping = false;
        StartCoroutine(CheckingRaycastDelay());
    }

    private void FixedUpdate(){
        if (isMoving){
            if (!isJumping)
                velocity.x = isGround ? direction * SPEED : 0;
            else
                velocity.x = direction * AIR_SPEED;
            velocity.y = rg.velocity.y;
            rg.velocity = velocity;
        }
        else if(!isJumping){
            rg.velocity = Vector2.zero;
        }
    }
    public virtual void Raycasting(){
        hit =  Physics2D.Raycast(BlockRaycastPos.position, Vector2.right * direction, DISTANCE_BLOCK, 1 << LayerMask.NameToLayer(TAG.TAG_GROUND));
        if (hit.collider != null){
            direction *=-1;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
        foreach(Transform RaycastPos in PosRaycastsDowns){
            isGround = Physics2D.Raycast(RaycastPos.position, Vector2.down, DISNTANCE_GROUND, 1 << LayerMask.NameToLayer(TAG.TAG_GROUND));
            if (isGround){
                if (isJumping && Time.time - prevJumpTime > JUMP_TIME)
                    isJumping = false;
                break;
            }
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

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.transform.gameObject.layer == 10  && !isJumping){
            CheckHeight(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 10 && !isJumping)
        {
            CheckHeight(collision);
        }
    }

    private IEnumerator Jumping(){
        float time = 0;
        while (time < JUMP_TIME){
            rg.velocity = new Vector2(direction * SPEED, JUMP_FORCE);
            time += Time.deltaTime;
            yield return null;
        }
        isJumping = false;
    }
    private bool CheckHeight(Collider2D collision)
    {
        float heigth = 0;
        bool canJump = false;
        heigth = collision.transform.position.y + (collision.bounds.size.y / 2f);
        Debug.Log(heigth - transform.position.y);
        if (heigth - transform.position.y < MAXJUMP_DISTANCE && !isJumping){
            prevJumpTime = Time.time;
            isJumping = true;
            canJump = true;
            rg.AddForce(Vector2.up * JUMP_FORCE);
        }
        return canJump;
    }
}
