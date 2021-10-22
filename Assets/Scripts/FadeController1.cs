using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeController1 : MonoBehaviour
{
    public static FadeController1 instace;
    [SerializeField] private Image imageFade;
    [SerializeField]
    private Coroutine Fading;
    private TextMeshProUGUI textMesh;
    private void Awake()
    {
        instace = this;
    }
    public void SetFade(Color color, float duration, bool Return = true, string text = "")
    {
        if (Fading != null) StopCoroutine(Fading);
        Fading = StartCoroutine(onFading(color, duration, Return, text));
    }
    IEnumerator onFading(Color color, float duration, bool Return, string text)
    {
        Image IMG = imageFade;
        Color initialColor = IMG.color;
        float timeElpse = 0;
        while (timeElpse <= duration)
        {
            IMG.color = Color.Lerp(initialColor, color, timeElpse / duration);
            timeElpse += Time.deltaTime;
            yield return null;
        }
        IMG.color = color;
        timeElpse = 0;
        if (Return)
        {
            if (text.Length > 0)
            {
                Color initalColor = new Color(1, 1, 1, 0);
                Color endColor = new Color(1, 1, 1, 1);
                textMesh.text = text;
                textMesh.color = initalColor;
                float extraTime = 0;
                while (timeElpse < 0.5f){
                    textMesh.color = Color.Lerp(initalColor, endColor, extraTime / 0.5f);
                    extraTime += Time.deltaTime;
                    yield return null;
                }
                yield return duration;
            }
            yield return new WaitForSeconds(duration);
            color = new Color(0, 0, 0, 0);
            initialColor = IMG.color;
            while (timeElpse <= duration)
            {
                IMG.color = Color.Lerp(initialColor, color, timeElpse / duration);
                timeElpse += Time.deltaTime;
                yield return null;
            }
            IMG.color = color;
        }

    }
    private void OnDestroy()
    {
        instace = null;
    }
}
