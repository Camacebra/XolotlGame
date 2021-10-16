using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private const string TRIGGET = "enter";

    bool isStarted = false;
    private Animator Anim;
    private void Awake()
    {
        Anim = GetComponent<Animator>();
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
}
