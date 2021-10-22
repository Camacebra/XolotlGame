using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrener : MonoBehaviour
{
    [SerializeField] private float Duration;
    [SerializeField] private Transform EndPos;
    private bool HasDrener = false;
    private Vector3 initalPos;
    private void Awake()
    {
        initalPos = transform.position;
    }
    public void WaterDraner(){
        if (!HasDrener){
            HasDrener = true;
            StartCoroutine(ScalingAnim());
        }
    }
    IEnumerator ScalingAnim(){
        float timeElpse = 0;
        Vector3 initalScale = transform.localScale;
        Vector3 EndScale = new Vector3(initalScale.x, 0, initalScale.z);
        while (timeElpse < Duration){
            transform.localScale = Vector3.Lerp(initalScale, EndScale, timeElpse / Duration);
            transform.position = Vector3.Lerp(initalPos, EndPos.position, timeElpse / Duration);
            timeElpse += Time.deltaTime;
            yield return null;
        }
        transform.localScale = EndScale;
    }
}
