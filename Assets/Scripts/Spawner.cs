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
        Vector2 pos = transform.position;
        int count = 0;
        for (int i = 0; i < level.baseSoulsNb; i++)
        {
            baseSouls.Add(Instantiate(baseSoul, new Vector2(pos.x + count * 2, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
            count++;
        }   
        for (int i = 0; i < level.waterSoulsNb; i++)
        {
            waterSouls.Add( Instantiate(waterSoul, new Vector2(pos.x + count * 2, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
            count++;
        }   
        for (int i = 0; i < level.warriorSoulsNb; i++)
        {
            warriorSouls.Add( Instantiate(warriorSoul, new Vector2(pos.x + count * 2, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
            count++;
        }   
        for (int i = 0; i < level.childSoulsNb; i++)
        {
            childSouls.Add( Instantiate(childSoul, new Vector2(pos.x + count * 2, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
            count++;
        }
    }

    public void CommandSouls(int i, Vector2 pos, float distance)
    {
        switch (i)
        {
            case 0:
                foreach (AI_Base soul in baseSouls)
                {
                    if (Vector2.Distance(pos, (Vector2)soul.transform.position) < distance)
                    {
                        soul.MovementSwitch();
                    }
                }
                break;
            case 1:
                foreach (AI_Base soul in waterSouls)
                {
                    if (Vector2.Distance(pos, (Vector2)soul.transform.position) < distance)
                    {
                        soul.MovementSwitch();
                    }
                }
                break;
            case 2:
                foreach (AI_Base soul in warriorSouls)
                {
                    if (Vector2.Distance(pos, (Vector2)soul.transform.position) < distance)
                    {
                        soul.MovementSwitch();
                    }
                }
                break;
            case 3:
                foreach (AI_Base soul in childSouls)
                {
                    if (Vector2.Distance(pos, (Vector2)soul.transform.position) < distance)
                    {
                        soul.MovementSwitch();
                    }
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
