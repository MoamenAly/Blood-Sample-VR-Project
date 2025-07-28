using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornkeyPAnel : MonoBehaviour
{
    public UiTimer uitimer;
    public ScoreManager scoreManager;

    private Coroutine scoreCoroutine;

    void Start()
    {
        uitimer.OnTimeFinishedAction += TornkeyLogic;
    }

    private void TornkeyLogic()
    {
        // Start logic when timer finishes
        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);

        scoreCoroutine = StartCoroutine(ScoreAfterDelay());
    }

    private IEnumerator ScoreAfterDelay()
    {
        // Wait 2 seconds before starting to give score
        yield return new WaitForSeconds(2f);

        float elapsed = 0f;
        float scoreRate = 1f; // Add 1 point per second, you can change this

        while (true)
        {
            elapsed += Time.deltaTime;

            // Add score every full second
            if (elapsed >= 1f)
            {
                scoreManager.DecreaseScore(Mathf.FloorToInt(elapsed * scoreRate));
                elapsed = 0f;
            }

            yield return null;
        }
    }

    public void StopScoring()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }
    }
}
