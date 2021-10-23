using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private const string TRIGGET = "enter";

    bool isStarted = false;
    [SerializeField] GameObject controls;
    private Animator Anim;
    private PlayerMovement player;
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        if (!isStarted && Input.anyKeyDown){
            isStarted = true;
            Anim.SetTrigger(TRIGGET);
        }
    }


    public void LoadLevel(int level){
        SceneManager.LoadScene(level);
    }

    public void StarGame()
    {
        controls.SetActive(true);
        player.Awaken();
        
    }
    public void PlayAudio()
    {
        Helpers.AudioManager.instance.PlayClip("start");
    }
}
