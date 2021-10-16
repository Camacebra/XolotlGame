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
    public delegate void AddSoul();
    public static AddSoul onAddSoul;
    public static LevelManager Instance;
    public bool isPause;
    [SerializeField]private Level[] levels;
    private int currentLevel;
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        isPause = false;
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public Level GetCurrentLevel(){
        return levels[currentLevel];
    }

    public void FinishSoul(TypeSoul type){
        onAddSoul?.Invoke();
    }
    public void LoadLevel(bool next){
        currentLevel = Mathf.Clamp(currentLevel + (next ? 1 : 0), 0, SceneManager.sceneCount);
        SceneManager.LoadScene(currentLevel+1);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
