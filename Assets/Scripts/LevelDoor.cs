using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    private const float ANIM_DURATION = 2;
    private int countSoulsNormal,
                countSoulsWater,
                countSoulsWarrior,
                countSoulsKid;
    private Level currentLevel;
    private bool Opened;
    [SerializeField] private Transform EndPos;
    void Start(){
        EndPos.transform.parent = null;
        Opened = false;
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        LevelManager.onAddSoul += AddSoul;
    }
    public void AddSoul(LevelManager.TypeSoul type){
        if (Opened) return;
        Debug.Log(type);
        switch (type) {
            case LevelManager.TypeSoul.Normal:
                countSoulsNormal++;
                break;
            case LevelManager.TypeSoul.Water:
                countSoulsWater++;
                break;
            case LevelManager.TypeSoul.Warrior:
                countSoulsWarrior++;
                break;
            case LevelManager.TypeSoul.Kid:
                countSoulsKid++;
                break;
        }
        if (countSoulsNormal < currentLevel.baseSoulsNb) return;
        if (countSoulsWarrior < currentLevel.warriorSoulsNb) return;
        if (countSoulsWater < currentLevel.waterSoulsNb) return;
        if (countSoulsKid < currentLevel.childSoulsNb) return;
        Opened = true;
        StartCoroutine(AnimMovement());
    }
    IEnumerator AnimMovement(){
        float timeElpse = 0;
        Vector3 intialPos = transform.position;
        while (timeElpse < ANIM_DURATION){
            transform.position = Vector3.Lerp(intialPos, EndPos.position, timeElpse / ANIM_DURATION);
            timeElpse += Time.deltaTime;
            yield return null;
        }
    }
}
