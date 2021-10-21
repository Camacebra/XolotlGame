using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController1 : MonoBehaviour
{
    public static FadeController1 instace;
    [SerializeField] private Image imageFade;
    private Coroutine Fading;

    private void Awake()
    {
        instace = this;
    }
    public void SetFade(Color color, float duration, bool Return = true)
    {
        if (Fading != null) StopCoroutine(Fading);
        Fading = StartCoroutine(onFading(color, duration, Return));
    }
    IEnumerator onFading(Color color, float duration, bool Return)
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
