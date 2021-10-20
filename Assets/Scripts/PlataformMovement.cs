using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMovement : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private float Speed = 10;
    [SerializeField] private bool Loop;
    [SerializeField] private float StopTime = 0;
    [SerializeField] private bool CauseDMG = false;
    private const float RadiusPath = 0.1f;
    private Vector3[] PathsPlataforms;
    private Vector3 targetPoint;
    private Collider2D myCollider;
    private int currentPosition;
    private float currentSpeed;
    void Start(){
        myCollider = GetComponent<Collider2D>();
        currentPosition = 0;
        PathsPlataforms = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++){
            PathsPlataforms[i] = transform.GetChild(i).position;
        }
        foreach (Transform trans in transform)
            Destroy(trans.gameObject);
        StartCoroutine(MovementPlataform());
    }
    IEnumerator MovementPlataform()
    {
        while (enabled){
            currentSpeed = Speed * Time.deltaTime;
            targetPoint = PathsPlataforms[currentPosition];
            if (Vector3.Distance(transform.position, targetPoint) < RadiusPath){
                yield return new WaitForSeconds(StopTime);
                if (currentPosition < PathsPlataforms.Length - 1) currentPosition++;
                else if (Loop)
                    currentPosition = 0;
                else break;
            }
            if (currentPosition >= PathsPlataforms.Length) break;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, currentSpeed);
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (CauseDMG) return;
        float distance = collision.transform.position.y + (collision.collider.bounds.size.y / 2) - (transform.position.y - (myCollider.bounds.size.y/2));
        //Debug.Log(distance);
        if (distance>1)
            collision.transform.parent = transform;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CauseDMG) return;
        if (collision.collider.transform.parent = transform)
            collision.transform.parent = null;
    }
}
