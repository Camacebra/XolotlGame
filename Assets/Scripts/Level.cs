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
                soulsFinish;
    [TextArea(15, 2)]
    public string Text;
    public Transform RespawnPos;
    public GameObject LevelPrefab;
    public Transform[] KeySpawn;
}
