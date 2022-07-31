using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadedOverlay : MonoBehaviour
{
    [Header("General")]
    public float FadeOutTime = 1f;
    public float FadeInTime = 1f;
    public float FadePauseTime = 0.4f;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetFadeProgress(0.0f);
    }

    public void ExecuteFadeSequence(Action fadeOutCallback = null, Action fadeInCallback = null, bool fadeIn = true)
    {
        StartCoroutine(FadeCoroutine(fadeOutCallback, fadeInCallback, fadeIn));
    }

    public void ExecuteFadeInSequence(Action fadeInCallback = null)
    {
        StartCoroutine(FadeInCoroutine(fadeInCallback));
    }

    private void SetFadeProgress(float fade)
    {
        _canvasGroup.alpha = fade;
    }

    private IEnumerator FadeCoroutine(Action fadeOutCallback, Action fadeInCallback, bool fadeIn)
    {
        float[] fadeTime = new float[] { FadeOutTime, FadeInTime };

        for (int i = 0; i <= (fadeIn ? 1 : 0); i++)
        {
            bool isFadeIn = i == 1;

            float time = 0.0f;

            while (time < 1.0f)
            {
                time += Time.deltaTime / fadeTime[i];

                float progress = Mathf.Lerp(0.0f, 1.0f, time);
                SetFadeProgress(isFadeIn ? 1.0f - progress : progress);

                yield return null;
            }

            SetFadeProgress(isFadeIn ? 0.0f : 1.0f);

            if (i == 0 && fadeOutCallback != null) fadeOutCallback();

            if (i == 0)
            {
                float time1 = 0.0f;
                while (time1 < 1.0f)
                {
                    time1 += Time.deltaTime / FadePauseTime;
                    yield return null;
                }
            }
        }


        if (fadeIn && fadeInCallback != null) fadeInCallback();
    }

    private IEnumerator FadeInCoroutine(Action fadeInCallback)
    {
        float time = 0.0f;

        while (time < 1.0f)
        {
            time += Time.deltaTime / FadeInTime;

            float progress = Mathf.Lerp(1.0f, 0.0f, time);
            SetFadeProgress(progress);

            yield return null;
        }

        if (fadeInCallback != null) fadeInCallback();
    }

    public static FadedOverlay Instance { get; private set; }
}