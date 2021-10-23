using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Souls : MonoBehaviour
{
    private const float ANIM_DURATION = 0.5f;
    public delegate void EndTIme();
    public static EndTIme OnEndTime;
    TextMeshProUGUI textSoulsCount, timeCount, EndLevel;
    private GameObject RestartBtn, NextLevel;
    private int currentSouls;
    private bool isCounting = false;
    private float timer;
    private Level currentLevel;
    private Coroutine CorEndLevel;
    private void Awake(){
        textSoulsCount = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        EndLevel = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timeCount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        RestartBtn = EndLevel.transform.GetChild(3).gameObject;
        NextLevel = EndLevel.transform.GetChild(4).gameObject;
        foreach (Transform child in EndLevel.transform)
            child.gameObject.SetActive(false);
        EndLevel.transform.localScale = Vector3.zero;
        EndLevel.gameObject.SetActive(false);
        OnEndTime += EndTimeEvent;
    }
    private void Start(){
        currentSouls = 0;
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        //timer = currentLevel.time;
        textSoulsCount.text = "0/" + currentLevel.soulsFinish.ToString();
        timeCount.text = "Time: " + timer.ToString();
        LevelManager.onAddSoul = AddSoul;
        isCounting = true;
    }
    private void Update(){
        if (isCounting){
            timer -= Time.deltaTime;
            timeCount.text = timeCount.text = "Time: " + ((int)timer % 60).ToString();
            if (timer <= 0){
                isCounting = false;
                OnEndTime?.Invoke();
            }
        }
    }

    void AddSoul(LevelManager.TypeSoul type){
        if (isCounting){
            currentSouls++;
            textSoulsCount.text = currentSouls.ToString() + "/" + currentLevel.soulsFinish.ToString();
            if (currentSouls >= currentLevel.soulsFinish)
            {
                isCounting = false;
                if (CorEndLevel != null) StopCoroutine(CorEndLevel);
                CorEndLevel = StartCoroutine(EndLevelAnim());
            }
        }
    }

    void EndTimeEvent(){
        if (CorEndLevel != null) StopCoroutine(CorEndLevel);
        CorEndLevel = StartCoroutine(EndLevelAnim());
    }
    IEnumerator EndLevelAnim(){
        LevelManager.Instance.isPause = true;
        float timeElpse = 0;
        float PorcentageLevel = (float)currentSouls / (float)currentLevel.soulsFinish;
        Debug.Log(PorcentageLevel);
        if (PorcentageLevel >= 0.5f){
            EndLevel.text = "LEVEL COMPLETE";
        }
        else{
            EndLevel.text = "FAIL LEVEL";
        }
        EndLevel.transform.localScale = Vector3.zero;
        EndLevel.gameObject.SetActive(true);
        StartCoroutine(DoScale(EndLevel.transform, new Vector3(1.25f,1.25f,1.25f), 0.25f));
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(DoScale(EndLevel.transform, Vector3.one, 0.2f));
        yield return new WaitForSeconds(0.25f);
        if (PorcentageLevel >= 0.5f){
            EndLevel.text = "LEVEL COMPLETE";
            EndLevel.transform.GetChild(0).gameObject.SetActive(true);
            EndLevel.transform.GetChild(0).localScale = Vector3.zero;
            StartCoroutine(DoScale(EndLevel.transform.GetChild(0), Vector3.one, 0.25f));
            yield return new WaitForSeconds(0.25f);
            if (PorcentageLevel >= 0.75f){
                EndLevel.transform.GetChild(1).gameObject.SetActive(true);
                EndLevel.transform.GetChild(1).localScale = Vector3.zero;
                StartCoroutine(DoScale(EndLevel.transform.GetChild(1), Vector3.one, 0.25f));
                yield return new WaitForSeconds(0.25f);
            }
            if (PorcentageLevel >= 1){
                EndLevel.transform.GetChild(2).gameObject.SetActive(true);
                EndLevel.transform.GetChild(2).localScale = Vector3.zero;
                StartCoroutine(DoScale(EndLevel.transform.GetChild(2), Vector3.one, 0.25f));
                yield return new WaitForSeconds(0.25f);
            }
        }
        yield return new WaitForSeconds(0.25f);
        if (PorcentageLevel >= 0.5f){
            EndLevel.text = "LEVEL COMPLETE";
            NextLevel.gameObject.SetActive(true);
            NextLevel.transform.localScale = Vector3.zero;
            StartCoroutine(DoScale(NextLevel.transform, Vector3.one, 0.25f));
            RestartBtn.gameObject.SetActive(true);
            RestartBtn.transform.localScale = Vector3.zero;
            StartCoroutine(DoScale(RestartBtn.transform, Vector3.one, 0.25f));
        }
        else{
            EndLevel.text = "FAIL LEVEL";
            NextLevel.gameObject.SetActive(false);
            RestartBtn.gameObject.SetActive(true);
            RestartBtn.transform.localScale = Vector3.zero;
            StartCoroutine(DoScale(RestartBtn.transform, Vector3.one, 0.25f));
        }
    }


    IEnumerator DoScale(Transform obj, Vector3 endScale, float duration){
        float timeElpse = 0;
        Vector3 initalScale = obj.transform.localScale;
        while (timeElpse < duration){
            obj.localScale = Vector3.Lerp(initalScale, endScale, timeElpse / duration);
            timeElpse += Time.deltaTime;
            yield return null;
        }
        obj.localScale = endScale;
    }
}
