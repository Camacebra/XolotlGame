using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    [SerializeField] private LayerMask layer;
    [HideInInspector] public string direction;
    [SerializeField] private bool isActive;
    [SerializeField] private float duration = 2f, maxDistance = 30f;
    private SpriteRenderer renderer;
    private RaycastHit2D hit;
    private float endPoint, progress = 0, tempDuration = 0;

    public bool DoingCoroutine { get; private set; }

    void Start()
    {
        endPoint = 0;
        renderer = GetComponent<SpriteRenderer>();
        GetEndPoint();
        if (endPoint == 0)
        {
            Debug.LogError("No wall found!");
        }
        if (isActive)
        {
            Vector2 size = new Vector2(endPoint, renderer.size.y);
            renderer.size = size;
        }
      
    }
    
    private void GetEndPoint()
    {
        switch (direction)
        {
            case "Right":
                hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance, layer);
                if (hit)
                    endPoint = Vector2.Distance(transform.position, hit.point);
                break;
            case "Down":
                hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance,  layer);
                if (hit)
                    endPoint = Vector2.Distance(transform.position, hit.point); break;
            case "Left":
                hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance,layer);
                if (hit)
                    endPoint = Vector2.Distance(transform.position, hit.point); break;
            case "Up":
                hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance, layer);
                if (hit)
                    endPoint = Vector2.Distance(transform.position, hit.point); break;
            default:
                break;
        }
    }

    public void Activate()
    {
        StopAllCoroutines();


        if (isActive)
        {
            if (DoingCoroutine)
            {
                tempDuration = renderer.size.x * duration / endPoint;

            }

            else
            {
                tempDuration = duration;
            }
            StartCoroutine(LerpFunction(0, tempDuration));
            isActive = false;
        }
        else
        {
            if (DoingCoroutine)
            {
                tempDuration = duration - renderer.size.x * duration / endPoint;
            }

            else
            {
                tempDuration = duration;
            }
            StartCoroutine(LerpFunction(endPoint, tempDuration));
            isActive = true;
        }
        DoingCoroutine = true;

    }

    // Update is called once per frame
    void Update()
    {
    }


    
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.right * maxDistance) ;
    }
    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = renderer.size.x;
        Vector2 _size = new Vector2(startValue, renderer.size.y);
        while (time < duration)
        {
            _size.x = Mathf.Lerp(startValue, endValue, time / duration);
            renderer.size = _size;
            time += Time.deltaTime;
            yield return null;
        }
        DoingCoroutine = false;
    }
}
