using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Renderer rend;
    private int currentMode;
    private Spawner spawn;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        spawn = GameObject.Find("Spawner").GetComponent<Spawner>();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeMode();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAction();
        }
    }

    private void DoAction()
    {
        spawn.CommandSouls(currentMode);

    }

    private void ChangeMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMode = currentMode > 0 ? currentMode - 1 : currentMode;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentMode = currentMode < 3 ? currentMode + 1 : currentMode;

        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        switch (currentMode)
        {
            case 0:
                rend.material.color = Color.white;
                break;
            case 1:
                rend.material.color = Color.blue;
                break;
            case 2:
                rend.material.color = Color.red;
                break;
            case 3:
                rend.material.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
