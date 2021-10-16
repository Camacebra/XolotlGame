using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SoulsSpawn {
        public GameObject Soul;
        public Transform[] SpawnPosition;

    }
    private List<AI_Base> baseSouls,
                        waterSouls,
                        warriorSouls,
                        childSouls;
    [SerializeField] private SoulsSpawn[] Souls;
    [SerializeField] private Vector3 offset;
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
        int count = 0;
        int SoulPerSpawn = level.baseSoulsNb / Souls[0].SpawnPosition.Length;
        for (int i = 0; i < Souls[0].SpawnPosition.Length; i++){
            count = 0;
            for (int j = 0; j < SoulPerSpawn; j++){
                baseSouls.Add(Instantiate(Souls[0].Soul, Souls[0].SpawnPosition[i].position + offset * count, Quaternion.identity, transform).GetComponent<AI_Base>());
                count++;
            }
        }
        if (Souls[1].SpawnPosition.Length > 0){
            SoulPerSpawn = level.waterSoulsNb / Souls[1].SpawnPosition.Length;
            for (int i = 0; i < Souls[1].SpawnPosition.Length; i++)
            {
                count = 0;
                for (int j = 0; j < SoulPerSpawn; j++)
                {
                    waterSouls.Add(Instantiate(Souls[1].Soul, Souls[1].SpawnPosition[i].position + offset * count, Quaternion.identity, transform).GetComponent<AI_Base>());
                    count++;
                }
            }
        }
        if (Souls[2].SpawnPosition.Length > 0){
            SoulPerSpawn = level.warriorSoulsNb / Souls[2].SpawnPosition.Length;
            for (int i = 0; i < Souls[2].SpawnPosition.Length; i++)
            {
                count = 0;
                for (int j = 0; j < SoulPerSpawn; j++)
                {
                    warriorSouls.Add(Instantiate(Souls[2].Soul, Souls[2].SpawnPosition[i].position + offset * count, Quaternion.identity, transform).GetComponent<AI_Base>());
                    count++;
                }
            }
        }
        if (Souls[3].SpawnPosition.Length > 0){
            SoulPerSpawn = level.childSoulsNb / Souls[3].SpawnPosition.Length;
            for (int i = 0; i < Souls[3].SpawnPosition.Length; i++)
            {
                count = 0;
                for (int j = 0; j < SoulPerSpawn; j++)
                {
                    childSouls.Add(Instantiate(Souls[3].Soul, Souls[3].SpawnPosition[i].position + offset * count, Quaternion.identity, transform).GetComponent<AI_Base>());
                    count++;
                }
            }
        }
        

        //for (int i = 0; i < level.waterSoulsNb; i++)
        //{
        //    waterSouls.Add( Instantiate(waterSoul, new Vector2(pos.x + count * 1.5f, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
        //    count++;
        //}   
        //for (int i = 0; i < level.warriorSoulsNb; i++)
        //{
        //    warriorSouls.Add( Instantiate(warriorSoul, new Vector2(pos.x + count * 1.5f, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
        //    count++;
        //}   
        //for (int i = 0; i < level.childSoulsNb; i++)
        //{
        //    childSouls.Add( Instantiate(childSoul, new Vector2(pos.x + count * 1.5f, pos.y), Quaternion.identity, transform).GetComponent<AI_Base>());
        //    count++;
        //}
    }

    public void CommandSouls(int i, Vector2 pos, float distance)
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
