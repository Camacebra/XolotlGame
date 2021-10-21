using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    private PlayerActions player;
    [SerializeField] private Image[] uiModes;
    [SerializeField] private Color selected, deselected;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMode();
    }

    private void CheckMode()
    {
        for (int i = 0; i < uiModes.Length; i++)
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
