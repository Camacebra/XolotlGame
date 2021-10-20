using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public enum TypeFX{
        Fade,
        Pixel
    }
    public static FadeController instance;
    [Range(0, 1)]
    [SerializeField] private float param = 1f;
    [Range(0, 1)]
    [SerializeField] float fade = 1;
    private Material material;
    private Coroutine fadeCorrutine;
    private void Awake(){
        material = new Material(Shader.Find("Unlit/CameraFX"));
        instance = this;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination){
        material.SetFloat("_p", param);
        material.SetFloat("_fade", fade);
        Graphics.Blit(source, destination, material);
    }
    public void Fade(float time, bool isIn, TypeFX type){
        if (fadeCorrutine != null)
            StopCoroutine(fadeCorrutine);
        fadeCorrutine = StartCoroutine(FadeAnim(time, isIn, type));
    }
    IEnumerator FadeAnim(float time, bool isIn, TypeFX type)
    {
        switch (type){
            case TypeFX.Fade:
                float initialFade = fade;
                float endFade = isIn ? 1 : 0;
                float timeElpse = 0;
                while (timeElpse < time){
                    fade = Mathf.Lerp(initialFade, endFade, timeElpse / time);
                    timeElpse += Time.deltaTime;
                    yield return null;
                }
                break;
            case TypeFX.Pixel:
                float initialParam = param;
                float endParam = isIn ? 0.3f : 5.2f;
                float timeElpse2 = 0;
                while (timeElpse2 < time){
                    param = Mathf.Lerp(initialParam, endParam, timeElpse2 / time);
                    timeElpse2 += Time.deltaTime;
                    yield return null;
                }
                break;
        }
        
       
        
    }
}
