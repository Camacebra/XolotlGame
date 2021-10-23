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
    [SerializeField] private bool canIncreaseProgress;
    [SerializeField] private GameObject prompt;
    private bool hasIncreasedProgress;
    private Level level;
    private PlayerActions player;
    private const float MAX_X_DIST = 15F, MAX_Y_DIST = 5F;
    void Start()
    {
        baseSouls = new List<AI_Base>();
        waterSouls = new List<AI_Base>();
        warriorSouls = new List<AI_Base>();
        childSouls = new List<AI_Base>();
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
        level = LevelManager.Instance.GetCurrentLevel();
        SpawnSouls();
    }

    public void SpawnSouls()
    {
        int count = 0;
        int SoulPerSpawn = level.baseSoulsNb / Souls[0].SpawnPosition.Length;
        for (int i = 0; i < Souls[0].SpawnPosition.Length; i++){
            count = 0;
            for (int j = 0; j < SoulPerSpawn; j++){
                baseSouls.Add(Instantiate(Souls[0].Soul, Souls[0].SpawnPosition[i].position + offset 
                                        * count, Quaternion.identity, transform).GetComponent<AI_Base>());
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
    public void DestroySouls()
    {
        foreach (AI_Base soul in baseSouls)
        {
            Destroy(soul.gameObject);
        }
        baseSouls.Clear();
        foreach (AI_Base soul in warriorSouls)
        {
            Destroy(soul.gameObject);
        }
        warriorSouls.Clear();
        foreach (AI_Base soul in waterSouls)
        {
            Destroy(soul.gameObject);
        }
        waterSouls.Clear();
        foreach (AI_Base soul in childSouls)
        {
            Destroy(soul.gameObject);
        }
        childSouls.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == player.name)
        {
            AwakeSouls();
            if (canIncreaseProgress && !hasIncreasedProgress &&
                Vector2.Distance(player.transform.position, transform.position) < 5f)
            {
                hasIncreasedProgress = true;
                player.IncreaseProgress();
                if (prompt != null)
                    prompt.SetActive(true);
            }
        }
    }

    private void AwakeSouls()
    {
        player.CanBark = true;
        Vector2 pos = player.transform.position;
        Vector2 pos2;
        float distx = 0;
        float disty = 0;
        foreach (AI_Base soul in baseSouls){
            pos2 = soul.transform.position;
            distx = Mathf.Abs(pos.x - pos2.x);
            disty = Mathf.Abs(pos.y - pos2.y);
            if (distx < MAX_X_DIST && disty < MAX_Y_DIST){
                soul.Activate();
            }
        }
        foreach (AI_Base soul in warriorSouls)
        {
            pos2 = soul.transform.position;
            distx = Mathf.Abs(pos.x - pos2.x);
            disty = Mathf.Abs(pos.y - pos2.y);
            if (distx < MAX_X_DIST && disty < MAX_Y_DIST)
            {
                soul.Activate();
            }
        }
        foreach (AI_Base soul in waterSouls)
        {
            pos2 = soul.transform.position;
            distx = Mathf.Abs(pos.x - pos2.x);
            disty = Mathf.Abs(pos.y - pos2.y);
            if (distx < MAX_X_DIST && disty < MAX_Y_DIST)
            {
                soul.Activate();
            }
        }
        foreach (AI_Base soul in childSouls)
        {
            pos2 = soul.transform.position;
            distx = Mathf.Abs(pos.x - pos2.x);
            disty = Mathf.Abs(pos.y - pos2.y);
            if (distx < MAX_X_DIST && disty < MAX_Y_DIST)
            {
                soul.Activate();
            }
        }
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
   
}
