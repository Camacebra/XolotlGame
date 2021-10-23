using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private const float ANIM_DURATION = 1.5f;

    private Vector3 EndPosition;
    private const string TAG_KEY = "Key";
    private bool hasKey = false;
    private void Awake(){
        EndPosition = transform.GetChild(0).position;
        Destroy(transform.GetChild(0).gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasKey && collision.CompareTag(TAG_KEY)){
            Debug.Log("ENTRAAAA");
            Debug.Log(collision.name);
            Helpers.AudioManager.instance.PlayClip("obtain");
            KeyPickUp key = collision.GetComponent<KeyPickUp>();
            if (key){
                collision.transform.parent = transform;
                hasKey = true;
                key.StartCoroutine(key.MoveAnimation(Vector3.zero, true));
                StartCoroutine(MovementAnim());
            }
        }
    }


    private IEnumerator MovementAnim(){
        float timeElpse = 0;
        Vector3 initalPosition = transform.position;
        yield return new WaitForSeconds(KeyPickUp.ANIM_DURATION);
        while (timeElpse < ANIM_DURATION){
            transform.position = Vector3.Lerp(initalPosition, EndPosition, timeElpse / ANIM_DURATION);
            timeElpse += Time.deltaTime;
            yield return null;
        }
        transform.position = EndPosition;
    }
}
