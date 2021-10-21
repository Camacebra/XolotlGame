using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level 
{
    public int  baseSoulsNb,
                waterSoulsNb,
                warriorSoulsNb,
                childSoulsNb,
                soulsFinish,
                time;
    public Transform RespawnPos;
    public GameObject LevelPrefab;
}
