using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class ClipStage
{
    public AudioClip guideClip;
    public int stage;
}

[DefaultExecutionOrder(-1)]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource narrationSource;

    public List<AudioClip> clipList;

    public List<ClipStage> clipStages;

    public Action onNarrationPlayed;

    private void Awake()
    {
        Instance = this;

        if (!gameObject.TryGetComponent<AudioSource>(out narrationSource))
        {
            narrationSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void DoneWithTheWelcoming()
    {
        StartCoroutine(DoneWithTheWelcomingCoroutine());
    }

    IEnumerator DoneWithTheWelcomingCoroutine()
    {
        for (int i = 0; i < clipList.Count; i++)
        {
            narrationSource.PlayOneShot(clipList[i]);
            yield return new WaitForSeconds(clipList[i].length);
        }

        //CanvasesManager.Instance.OnClick_OpenCanvasAfterDelay(5);

        //if (PlanetsManager.Instance.scifiSnap != null) PlanetsManager.Instance.scifiSnap.SetActive(true);
        //if (PlanetsManager.Instance.planetsGrabbables != null) PlanetsManager.Instance.planetsGrabbables.SetActive(true);
        //if (PlanetsManager.Instance.handPreview != null) PlanetsManager.Instance.handPreview.GetComponent<Animator>().enabled = true;

        //PlanetsManager.Instance.PlanetsPointerState(true);
    }

    //public async void DoneWithTheWelcoming()
    //{
    //    for (int i = 0; i < clipList.Count; i++)
    //    {
    //        narrationSource.PlayOneShot(clipList[i]);
    //        await Task.Delay((int)(clipList[i].length * 1000));
    //    }
    //    CanvasesManager.Instance.OnClick_OpenCanvasAfterDelay(5);
    //    if (PlanetsManager.Instance.scifiSnap != null) PlanetsManager.Instance.scifiSnap.SetActive(true);
    //    if (PlanetsManager.Instance.planetsGrabbables != null) PlanetsManager.Instance.planetsGrabbables.SetActive(true);
    //    PlanetsManager.Instance.PlanetsPointerState(true);
    //}

    public void PlayGuideClip(int stage)
    {
        if (stage < clipStages.Count)
        {
            narrationSource.PlayOneShot(clipStages[stage].guideClip);
        }
    }

    public IEnumerator PlayNarrationAndWaitCoroutine(AudioClip narrationClip, int actionDelay)
    {
        narrationSource.clip = narrationClip;
        narrationSource.Play();

        yield return new WaitForSeconds(actionDelay);

        onNarrationPlayed?.Invoke();

        yield return new WaitForSeconds(narrationSource.clip.length);
    }

    public void PlayNarration(AudioClip narrationClip, int actionDelay)
    {
        narrationSource.clip = narrationClip;
        narrationSource.Play();
    }

    public void StopNarration()
    {
        narrationSource.Stop();
    }

    //public async Task PlayNarrationAndWait(AudioClip narrationClip, int actionDelay)
    //{
    //    narrationSource.clip = narrationClip;
    //    narrationSource.Play();
    //    await Task.Delay(actionDelay * 1000);
    //    onNarrationPlayed?.Invoke();
    //    await Task.Delay((int)(narrationSource.clip.length * 1000));
    //}
}
