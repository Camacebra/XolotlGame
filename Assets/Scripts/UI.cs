using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    private PlayerActions player;
    [SerializeField] private Image[] uiModes;
    [SerializeField] private Color selected, deselected, disabled;
    private int active;
    private int tweenId;
    private Vector3 startPos, endPos;
    void Start()
    {
        startPos = uiModes[3].rectTransform.localPosition;
        endPos = uiModes[0].rectTransform.localPosition;
        active = 0;
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
        for (int i = 0; i < uiModes.Length; i++)
        {
            uiModes[i].color = disabled;
            uiModes[i].rectTransform.localPosition = startPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(endPos.x);
        CheckMode();
        CheckProgress();
    }

    private void CheckProgress()
    {
        if (active == player.currentProgress)
            return;
        active = player.currentProgress;
        EnableImage();
    }

    private void EnableImage()
    {
        tweenId = LeanTween.color(uiModes[active - 1].gameObject, selected, 0.6f).setOnComplete(MoveImage).id;
    }

    private void MoveImage()
    {
        LeanTween.cancel(tweenId);
        tweenId = LeanTween.moveLocalX(uiModes[active - 1].gameObject, endPos.x + 150f * (active - 1), 0.5f).id;
    }

    private void CheckMode()
    {
        for (int i = 0; i < player.currentProgress; i++)
        {
            if (i == player.currentMode)
            {
                uiModes[i].color = selected;
            }
            else
            {
                uiModes[i].color = deselected;
            }
        }
    }
}
