using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    private const string TAG_PLAYER = "Player";
    private bool CanAddRespawn = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG_PLAYER) && CanAddRespawn)
        {
            PlayerHealth respawnPropety = collision.GetComponent<PlayerHealth>();
            if (respawnPropety)
            {
                respawnPropety.GetRespawnPos(transform.position);
                CanAddRespawn = false;
                LevelManager.Instance.ChangeLevel();
            }
        }
    }
}
