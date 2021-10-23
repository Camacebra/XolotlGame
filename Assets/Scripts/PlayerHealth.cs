using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    private const float ParpadeoTime = 0.075f;
    private const float FADE_DURATION = 1.5f;
    [SerializeField] private Image[] ImgHealth;
    [SerializeField] private Sprite[] spriteHealths; //0 Sin vida 1 con vida
    [SerializeField] private float HitForce;
    [SerializeField] private float InvensibilityDuration;
    [HideInInspector] public bool isGettingHit;
    private Vector3 PosRespawn;
    private SpriteRenderer sprite;
    public int actualLifes;
    private bool canDMG;
    private Coroutine corrutienRespawn;
    private Rigidbody2D rg;
    private Spawner spawn;
    private PlayerMovement player;
    private void Awake(){
        rg = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerMovement>();
        PosRespawn = transform.position;
        canDMG = true;
        for (int i = 0; i < ImgHealth.Length; i++){
            ImgHealth[i].sprite = spriteHealths[1]; //asignar vida actual
        }
        actualLifes = ImgHealth.Length;
    }
    private void OnEnable()
    {
        spawn = FindObjectOfType<Spawner>(); 
    }
    public void GetDMG(Vector3 Pos, Vector2 velocity, int dmg)
    {
        Debug.Log("GET DMG");
        if (dmg > ImgHealth.Length-1){
            if (corrutienRespawn != null) StopCoroutine(corrutienRespawn);
            corrutienRespawn = StartCoroutine(Respawn());
        }
        else if (actualLifes > 0 && canDMG){
            //Helpers.AudioManager.instance.PlayClip(GameAudiosManager.AUDIOS.HURT, 0.9f, Random.Range(0.9f, 1.1f));
            Vector2 Force = Vector2.zero;
            if (velocity == Vector2.zero)
                Force = new Vector2((Pos.x > transform.position.x ? -1 : 1), 1) * HitForce;
            else
                Force = velocity * HitForce;
            ImgHealth[actualLifes - 1].sprite = spriteHealths[0];
            canDMG = false;
            isGettingHit = true;
            actualLifes -= dmg;
            if (actualLifes > 0){
                rg.velocity = Vector2.zero;
                rg.AddForce(Force, ForceMode2D.Impulse);
                StartCoroutine(Invensibility());
            }
            else{
                if (corrutienRespawn != null) StopCoroutine(corrutienRespawn);
                corrutienRespawn = StartCoroutine(Respawn());
            }
        }
    }
    public void GetRespawnPos(Vector3 Position){
        PosRespawn = Position;
    }
    IEnumerator Respawn()
    {
        //HISTOPTIME
        Time.timeScale = 0;
        sprite.color = Color.red;
        player.CanMove = false;
        rg.velocity = Vector2.zero;
        player.resetAnimation();
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1;
        FadeController1.instace.SetFade(Color.black, 1, true);
        yield return new WaitForSeconds(FADE_DURATION);
        if(spawn)
            spawn.DestroySouls();
        LevelManager.Instance.resetLevel();
        rg.velocity = Vector2.zero;
        transform.position = PosRespawn;
        actualLifes = ImgHealth.Length;
        for (int i = 0; i < ImgHealth.Length; i++){
            ImgHealth[i].sprite = spriteHealths[1];
        }
        sprite.color = Color.white;
        spawn.SpawnSouls();
        yield return new WaitForSeconds(0.5f);
        FadeController.instance.Fade(FADE_DURATION, true, FadeController.TypeFX.Fade);
        yield return new WaitForSeconds(FADE_DURATION);
        rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        isGettingHit = false;
        canDMG = true;
        player.CanMove = true;
    }

    IEnumerator Invensibility()
    {
        Time.timeScale = 0;
        sprite.color = Color.red;
        yield return new WaitForSecondsRealtime(0.15f);
        sprite.color = Color.white;
        Time.timeScale = 1;
        float timeElpse = 0;
        float timerFlick = 0;
        float timerGeting = 0;
        while (timeElpse < InvensibilityDuration)
        {
            if (timerFlick >= ParpadeoTime)
            {
                timerFlick = 0;
                sprite.enabled = !sprite.enabled;
            }
            if (timerGeting >= 0.5f && isGettingHit)
                isGettingHit = false;
            yield return null;
            timerGeting += Time.deltaTime;
            timerFlick += Time.deltaTime;
            timeElpse += Time.deltaTime;
        }
        sprite.enabled = true;
        canDMG = true;
    }
}
