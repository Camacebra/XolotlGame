using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> Keys = new List<GameObject>();
    [SerializeField]private Level[] levels;
    [SerializeField]private int currentLevel;
    private PlayerHealth player;
    [SerializeField] private GameObject globalLight, controls;
    [SerializeField] private GameObject[] hearts;
    [SerializeField ] private GameObject KeyPrefab;
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        player = FindObjectOfType<PlayerHealth>();
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
        Keys.Clear();
        if(currentLevel+1<levels.Length)
            FadeController1.instace.SetFade(Color.black, 1, true, levels[currentLevel].Text);
        else
            FadeController1.instace.SetFade(Color.black, 1, false, levels[currentLevel].Text, true);
        controls.SetActive(false);
        yield return new WaitForSeconds(1);
        globalLight.SetActive(true);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }
        levels[currentLevel].LevelPrefab.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        currentLevel++;
        if (currentLevel < levels.Length){
            player.transform.position = levels[currentLevel].RespawnPos.position;
            player.GetRespawnPos(levels[currentLevel].RespawnPos.position);
            levels[currentLevel].LevelPrefab.gameObject.SetActive(true);
            if(levels[currentLevel].KeySpawn.Length>0){
                for (int i = 0; i < levels[currentLevel].KeySpawn.Length; i++)
                {
                    Keys.Add(Instantiate(KeyPrefab, levels[currentLevel].KeySpawn[i].position, Quaternion.identity));
                }
                
            }
            player.gameObject.SetActive(true);
            player.StopMovement();
            Invoke("resetPlayer", 0.75f);

        }
    }
    void resetPlayer()
    {
        player.ContinueMovement();
    }
    public void resetLevel()
    {
        foreach(GameObject key in Keys.ToArray()){
            if (key)
                Destroy(key);
        }
        Keys.Clear();
        if (levels[currentLevel].KeySpawn.Length > 0){
            for (int i = 0; i < levels[currentLevel].KeySpawn.Length; i++)
            {
                Keys.Add(Instantiate(KeyPrefab, levels[currentLevel].KeySpawn[i].position, Quaternion.identity));
            }

        }
    }
}
