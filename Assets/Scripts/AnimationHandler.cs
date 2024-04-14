using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Image fadingPanel;
    [SerializeField] private Animator animator;
    private const float FADE_TIME_SECONDS = 2;
    private const int MIN_BLINK_GAP_TIME = 5;
    private const int MAX_BLINK_GAP_TIME = 8;
    private const string BLINK_TRIGGER = "blink";
    private const string HELLO_TRIGGER = "hello";
    private const string THUMBS_UP_TRIGGER = "thumbsup";
    private bool isAnimating;
    private bool isBlinking;
    void Start()
    {
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        Color imageColor = fadingPanel.color;
        float fadeDelta = GetFadeDelta();
        isAnimating = true;
        while (fadingPanel.color.a > 0)
        {
            imageColor.a -= fadeDelta;
            fadingPanel.color = imageColor;
            yield return new WaitForFixedUpdate();
        }
        isAnimating = false;
        fadingPanel.gameObject.SetActive(false);
    }
    private float GetFadeDelta()
    {
        return 1 / (FADE_TIME_SECONDS * 60);
    }

    private void Update()
    {
        if (!isBlinking)
            StartCoroutine(AnimateBlink());
    }
    private IEnumerator AnimateBlink()
    {
        float blinkGapTime = Random.Range(MIN_BLINK_GAP_TIME, MAX_BLINK_GAP_TIME);
        isBlinking = true;
        yield return new WaitForSeconds(blinkGapTime);
        yield return StartCoroutine(SetAnimationAndWait(BLINK_TRIGGER));
        isBlinking = false;
    }
    private IEnumerator SetAnimationAndWait(string trigger)
    {
        animator.SetTrigger(trigger);
        yield return new WaitUntil(AnimationStarted());
        yield return new WaitForSeconds(CurrentClipLength());
    }
    private Func<bool> AnimationStarted()
    {
        return () => animator.GetCurrentAnimatorClipInfo(0).Length > 0;
    }
    private float CurrentClipLength()
    {
        AnimatorClipInfo[] clipsInfo = animator.GetCurrentAnimatorClipInfo(0);
        return clipsInfo[0].clip.length;
    }

    public IEnumerator FadeOut()
    {
        fadingPanel.gameObject.SetActive(true);
        Color imageColor = fadingPanel.color;
        float fadeDelta = GetFadeDelta();
        isAnimating = true;
        while (fadingPanel.color.a < 1)
        {
            imageColor.a += fadeDelta;
            fadingPanel.color = imageColor;
            yield return new WaitForFixedUpdate();
        }
        isAnimating = false;
    }
    private IEnumerator AnimateCor(string trigger)
    {
        isAnimating = true;
        yield return StartCoroutine(SetAnimationAndWait(trigger));
        isAnimating = false;
    }
    public void AnimateHello()
    {
        if (!isAnimating)
            StartCoroutine(AnimateCor(HELLO_TRIGGER));
    }

    public void AnimateThumbsUp()
    {
        if (!isAnimating)
            StartCoroutine(AnimateCor(THUMBS_UP_TRIGGER));
    }

    public void AnimateBye()
    {
        if (!isAnimating)
            StartCoroutine(AnimateByeCor());
    }

    private IEnumerator AnimateByeCor()
    {
        yield return StartCoroutine(AnimateCor(HELLO_TRIGGER));
        yield return StartCoroutine(FadeOut());
        Application.Quit();
    }
}
