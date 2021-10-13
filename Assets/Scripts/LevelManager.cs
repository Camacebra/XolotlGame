using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum TypeSoul{
        Normal,
        Water,
        Warrior,
        Kid
    }
    public static LevelManager Instance;
    [SerializeField]private Level[] levels;
    private int currentLevel;
    private int currentSouls;
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public Level GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    public void FinishSoul(TypeSoul type){
        currentSouls++;
    }
}
