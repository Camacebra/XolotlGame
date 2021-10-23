using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public const float ANIM_DURATION = 0.2f;
    private const string OBJECT_SOUND = "obtain";
    private bool HasPickUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasPickUp && (collision.gameObject.layer == 8))
        {
            HasPickUp = true;
            transform.parent = collision.transform;
            Vector3 position = collision.transform.InverseTransformPoint(collision.bounds.center + (collision.bounds.size));
            Vector3 EndPosition = new Vector3(0, position.y, 0);
            StartCoroutine(MoveAnimation(EndPosition));
            if (Helpers.AudioManager.instance)
                Helpers.AudioManager.instance.PlayClip(OBJECT_SOUND);
        }
    }

    public IEnumerator MoveAnimation(Vector3 Position, bool DestroyOnEnd = false){
        float timeElpse = 0;
        Vector3 initalPos = transform.localPosition;
        while(timeElpse < ANIM_DURATION){
            transform.localPosition = Vector3.Lerp(initalPos, Position, timeElpse / ANIM_DURATION);
            timeElpse += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Position;
        if (DestroyOnEnd)
            Destroy(gameObject);
    }
}
