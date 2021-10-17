using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{

    private RaycastHit2D hit;
    [SerializeField] private LayerMask layer;
    [HideInInspector] public string direction;
    [SerializeField] private SpriteRenderer renderer;
    private float endPoint;
    private bool isActive = false;
    void Start()
    {
        GetEndPoint();
    }
    
    private void GetEndPoint()
    {
        switch (direction)
        {
            case "Right":
                hit = Physics2D.Raycast(transform.position, -transform.right, 100f, layer);
                endPoint = Vector2.Distance(transform.position, hit.point);
                break;
            case "Down":
                hit = Physics2D.Raycast(transform.position, -transform.right, 100f,  layer);
                endPoint = Vector2.Distance(transform.position, hit.point);
                break;
            case "Left":
                hit = Physics2D.Raycast(transform.position, -transform.right, 100f,layer);
                endPoint = Vector2.Distance(transform.position, hit.point);
                break;
            case "Up":
                hit = Physics2D.Raycast(transform.position, -transform.right, 100f, layer);
                endPoint = Vector2.Distance(transform.position, hit.point);
                break;
            default:
                break;
        }
    }

    public void Enable(Switch _switch)
    {
        if (!isActive)
        {
            StartCoroutine(LerpFunction(endPoint, 2f));
            isActive = true;
        }
    }
    public void Disable(Switch _switch)
    {
        if (!isActive)
        {
            StartCoroutine(LerpFunction(0f, 2f));
            isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDrawGizmos()
    {
        if (endPoint != 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
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
        isActive = false;
    }
}
