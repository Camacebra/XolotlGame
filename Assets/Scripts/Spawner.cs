using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<AI_Base> baseSouls,
                        waterSouls,
                        warriorSouls,
                        childSouls;
    [SerializeField] private GameObject baseSoul, 
                                        waterSoul, 
                                        warriorSoul, 
                                        childSoul;
    private Level level;
  
    void Start()
    {
        baseSouls = new List<AI_Base>();
        waterSouls = new List<AI_Base>();
        warriorSouls = new List<AI_Base>();
        childSouls = new List<AI_Base>();
        level = LevelManager.Instance.GetCurrentLevel();
        SpawnSouls();
    }

    private void SpawnSouls()
    {
        for (int i = 0; i < level.baseSoulsNb; i++)
        {
            baseSouls.Add(Instantiate(baseSoul, transform.position, Quaternion.identity, transform).GetComponent<AI_Base>());

        }   
        for (int i = 0; i < level.waterSoulsNb; i++)
        {
            waterSouls.Add( Instantiate(waterSoul, transform.position, Quaternion.identity, transform).GetComponent<AI_Base>());

        }   
        for (int i = 0; i < level.warriorSoulsNb; i++)
        {
            warriorSouls.Add( Instantiate(warriorSoul, transform.position, Quaternion.identity, transform).GetComponent<AI_Base>());

        }   
        for (int i = 0; i < level.childSoulsNb; i++)
        {
            childSouls.Add( Instantiate(childSoul, transform.position, Quaternion.identity, transform).GetComponent<AI_Base>());

        }
    }

    public void CommandSouls(int i)
    {
        switch (i)
        {
            case 0:
                foreach (AI_Base soul in baseSouls)
                {
                    soul.MovementSwitch();
                }
                break;
            case 1:
                foreach (AI_Base soul in waterSouls)
                {
                    soul.MovementSwitch();
                }
                break;
            case 2:
                foreach (AI_Base soul in warriorSouls)
                {
                    soul.MovementSwitch();
                }
                break;
            case 3:
                foreach (AI_Base soul in childSouls)
                {
                    soul.MovementSwitch();
                }
                break;
            default:
                break;
        }
    }
   
    // Update is called once per frame
    void Update()   
    {
        
    }
}
