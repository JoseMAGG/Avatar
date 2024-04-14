using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognizer : MonoBehaviour
{
    [SerializeField] private AnimationHandler animationHandler;
    private KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> triggerKeywords = new Dictionary<string, Action>();
    void Start()
    {
        FillTriggerKeywords();
        IntializeRecognizer();
    }
    private void FillTriggerKeywords()
    {
        triggerKeywords.Add("hola", AnimateHello);
        triggerKeywords.Add("adios", AnimateBye);
        triggerKeywords.Add("bien", AnimateThumbsUp);
    }
    private void AnimateHello()
    {
        animationHandler.AnimateHello();
    }
    private void AnimateThumbsUp()
    {
        animationHandler.AnimateThumbsUp();
    }

    private void AnimateBye()
    {
        animationHandler.AnimateBye();
    }
    private void IntializeRecognizer()
    {
        keywordRecognizer = new KeywordRecognizer(triggerKeywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Animate;
        keywordRecognizer.Start();
    }
    private void Animate(PhraseRecognizedEventArgs recognizedWord)
    {
        triggerKeywords[recognizedWord.text].Invoke();
    }
}
