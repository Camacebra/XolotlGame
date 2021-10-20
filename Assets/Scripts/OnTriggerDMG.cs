using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDMG : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] Vector2 Velocity = Vector2.zero;
    [SerializeField] private int DMG = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player){
                player.GetDMG(transform.position, Velocity, DMG);
            }
                
        }
    }
}
