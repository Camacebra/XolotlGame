using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public enum TypeSoul{
        Normal,
        Water,
        Warrior,
        Kid
    }
    public delegate void AddSoul(TypeSoul type);
    public static AddSoul onAddSoul;
    public static LevelManager Instance;
    public bool isPause;
    [SerializeField]private Level[] levels;
    [SerializeField]private int currentLevel;
    private PlayerMovement player;
    [SerializeField] private GameObject globalLight;
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        player = FindObjectOfType<PlayerMovement>();
        isPause = false;
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public Level GetCurrentLevel(){
        return levels[currentLevel];
    }

    public void FinishSoul(TypeSoul type){
        onAddSoul?.Invoke(type);
    }
    public void LoadLevel(bool next){
        currentLevel = Mathf.Clamp(currentLevel + (next ? 1 : 0), 0, SceneManager.sceneCount);
        SceneManager.LoadScene(currentLevel+1);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
    public void ChangeLevel()
    {
        StartCoroutine(ChangingAnim());
    }

    IEnumerator ChangingAnim()
    {
        FadeController1.instace.SetFade(Color.black, 1, true);
        yield return new WaitForSeconds(1);
        globalLight.SetActive(true);
        levels[currentLevel].LevelPrefab.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        currentLevel++;
        player.transform.position = levels[currentLevel].RespawnPos.position;
        levels[currentLevel].LevelPrefab.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
    }
}
