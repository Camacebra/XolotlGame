using System;
using System.Collections;
using UnityEngine;
public class AI_Base : MonoBehaviour
{
    public struct TAG {
        public const string TAG_GROUND = "Ground",
                            TAG_WATER = "Water",
                            TAG_BREACK = "Breackeable",
                            TAG_PLATAFORM = "Plataform";
    }

    private const string ANIM_WALK = "Walk",
                         ANIM_IDLE = "Idle";
    private const float DISTANCE_BLOCK = 1f,
                        DISNTANCE_GROUND = 0.25f,
                        SPEED = 2.5f,
                        ///JUMP//
                        AIR_SPEED = 1.5f,
                        JUMP_FORCE = 550,
                        MAXJUMP_DISTANCE = 1.5f,
                        JUMP_TIME = 0.2f;
    private const string WALK_SOUND = "walk-soul";
    [SerializeField] private Transform[] PosRaycastsDowns;
    [SerializeField] private Transform BlockRaycastPos;
    [SerializeField] private LayerMask Walkeable;
    public LevelManager.TypeSoul myTypeSoul;
    private Rigidbody2D rg;
    private Vector2 velocity;
    protected RaycastHit2D hit;
    protected float direction, prevJumpTime;
    public bool isGround, isWater, isJumping, hasBlocked, isActive;
    public delegate void callSouls();
    private bool isMoving { get;  set; }
    private AudioSource audioSource;
    private Animator anim;



    private void Awake(){
        rg = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        direction = 1;
        isJumping = false;
        isActive = false;
        hasBlocked = false;
        StartCoroutine(CheckingRaycastDelay());
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate(){
        if (isMoving && !LevelManager.Instance.isPause){
            if (!isJumping)
                velocity.x = isGround ? direction * SPEED : 0;
            else
                velocity.x = direction * AIR_SPEED;
            velocity.y = rg.velocity.y;
            rg.velocity = velocity;
            if(Helpers.AudioManager.instance)
                Helpers.AudioManager.instance.PlayClip(WALK_SOUND + UnityEngine.Random.Range(1, 5).ToString(), UnityEngine.Random.Range(0.25f, 0.5f), UnityEngine.Random.Range(0.9f, 1.5f), audioSource);
        }
        else if(!isJumping){
            rg.velocity = new Vector2(0, rg.velocity.y);
        }
    }
    public virtual void Raycasting(){
        hit =  Physics2D.Raycast(BlockRaycastPos.position, Vector2.right * direction, DISTANCE_BLOCK, Walkeable);
        if (hit.collider != null && !isJumping && !hit.collider.CompareTag(TAG.TAG_PLATAFORM) && !hit.collider.isTrigger){
            hasBlocked = true;
            direction *=-1;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
            hasBlocked = false;
        }
        foreach(Transform RaycastPos in PosRaycastsDowns){
            isGround = Physics2D.Raycast(RaycastPos.position, Vector2.down, DISNTANCE_GROUND, Walkeable);
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
        if (isActive)
        {
            if (Helpers.AudioManager.instance)
                Helpers.AudioManager.instance.PlayClip("bark" + (isMoving? "1": "2"));
            anim.Play(isMoving ? ANIM_IDLE : ANIM_WALK);
            isMoving = !isMoving;
        }
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
        if ((collision.transform.gameObject.layer == 10 || collision.transform.gameObject.layer == 9)  && !isJumping){
            CheckHeight(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.transform.gameObject.layer == 10 || collision.transform.gameObject.layer == 9) && !isJumping)
        {
            CheckHeight(collision);
        }
    }

    internal void Activate()
    {
        isActive = true;
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
        if (!isMoving) return false;
        if (hasBlocked) return false;
        if (LevelManager.Instance.isPause) return false;
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
