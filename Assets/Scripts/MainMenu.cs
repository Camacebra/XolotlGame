using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private const string TRIGGET = "enter";

    bool isStarted = false;
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
        player.Awaken();
    }
}
