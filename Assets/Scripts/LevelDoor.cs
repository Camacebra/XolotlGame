using System.Collections;
using UnityEngine;
using TMPro;

public class LevelDoor : MonoBehaviour
{
    private const float ANIM_DURATION = 2;
    //private int countSoulsNormal,
    //            countSoulsWater,
    //            countSoulsWarrior,
    //            countSoulsKid;
    private int countSouls;
    private Level currentLevel;
    private bool Opened;
    [SerializeField] private Transform EndPos;
    [SerializeField] private TextMeshPro text;
    void Start(){
        EndPos.transform.parent = null;
        Opened = false;
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        LevelManager.onAddSoul += AddSoul;
        countSouls = 0;
        text.text = "0/" + currentLevel.soulsFinish.ToString();
    }
    public void AddSoul(LevelManager.TypeSoul type){
        if (Opened) return;
        //Debug.Log(type);
        //switch (type) {
        //    case LevelManager.TypeSoul.Normal:
        //        countSoulsNormal++;
        //        break;
        //    case LevelManager.TypeSoul.Water:
        //        countSoulsWater++;
        //        break;
        //    case LevelManager.TypeSoul.Warrior:
        //        countSoulsWarrior++;
        //        break;
        //    case LevelManager.TypeSoul.Kid:
        //        countSoulsKid++;
        //        break;
        //}
        //if (countSoulsNormal < currentLevel.baseSoulsNb) return;
        //if (countSoulsWarrior < currentLevel.warriorSoulsNb) return;
        //if (countSoulsWater < currentLevel.waterSoulsNb) return;
        //if (countSoulsKid < currentLevel.childSoulsNb) return;
        countSouls++;
        text.text = countSouls.ToString() + "/" + currentLevel.soulsFinish.ToString();
        if (countSouls < currentLevel.soulsFinish) return;
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
