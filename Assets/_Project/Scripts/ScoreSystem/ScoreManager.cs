using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    //[SerializeField] int maxScore;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip increaseScoreClip;
    [SerializeField] AudioClip decreaseScoreClip;

    [SerializeField] UnityEvent2 OnIncreasedScore;
    [SerializeField] UnityEvent2 OnDecreasedScore;

    private int currentScore;

    private void Start()
    {
        SetScore();
    }

    [Button]
    public void IncreaseScore(int amountScore)
    {
        //if (currentScore >= maxScore) return;

        int targetScore = currentScore + amountScore;

        Debug.LogError("currentScore" + currentScore + "targetScore " + targetScore);
        DOTween.To(() => currentScore, x =>
        {
            currentScore = x;
            scoreTxt.text = currentScore.ToString();
        }, targetScore, 1f);

        if (!sfxSource.isPlaying)
            sfxSource.PlayOneShot(increaseScoreClip);

        OnIncreasedScore?.Invoke();
        ScoreDataHolder.score = targetScore;
    }

    [Button]
    public void DecreaseScore(int amountScore)
    {
        if (currentScore <= 0) return;

        int targetScore = Mathf.Max(currentScore - amountScore, 0);

        DOTween.To(() => currentScore, x =>
        {
            currentScore = x;
            scoreTxt.text = currentScore.ToString();
        }, targetScore, 1f);

        if (!sfxSource.isPlaying)
            sfxSource.PlayOneShot(decreaseScoreClip);

        OnDecreasedScore?.Invoke();
        ScoreDataHolder.score = targetScore;

    }

    public void SetScore()
    {
        currentScore = ScoreDataHolder.score;
        scoreTxt.text = ScoreDataHolder.score.ToString();
    }
}
