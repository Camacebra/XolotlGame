using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    private const string SOULS_TAG = "Soul";
    [SerializeField] private LevelManager.TypeSoul type;

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag(SOULS_TAG))
        {
            AI_Base ai = collision.gameObject.GetComponent<AI_Base>();
            if (ai && ai.myTypeSoul == type)
            {
                LevelManager.Instance.FinishSoul(type);
                Destroy(collision.gameObject);
            }
        }
    }
}
