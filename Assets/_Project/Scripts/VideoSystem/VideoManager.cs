using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public class VideoManager : MonoBehaviour
{


    [SerializeField] private VideoPlayer video;
    [FoldoutGroup("Video Controls")]
    [SerializeField] private Slider  videoSeekSlider;
    [FoldoutGroup("Video Controls")]
    [SerializeField] private Button PlayBtn, PauseBtn , SeekForward , SeekBackword;
    [FoldoutGroup("Video Controls")]
    [SerializeField] private TextMeshProUGUI currentTimeText, totalTimeText;


    // Start is called before the first frame update
    void Start()
    {
        SetSlider();

        PlayBtn.onClick.AddListener(PlayVideo);
        PauseBtn.onClick.AddListener(PauseVideo);

        SeekForward.onClick.AddListener(()=>{

            video.time += 3f;
        });

        SeekBackword.onClick.AddListener(() => {

            video.time -= 3f;
        });
        video.loopPointReached += OnVideoFinished;

    }

    // Update is called once per frame
    void Update()
    {
        if (video.frameCount > 0)
        {
            videoSeekSlider.value = video.frame;

            // Update current time display
            currentTimeText.text = FormatTime(video.time);

            // Update total duration display
            totalTimeText.text = FormatTime(video.length);
        }
    }
    private void SetSlider()
    {
        ulong frameCount = video.frameCount;
        videoSeekSlider.maxValue = frameCount;
    }


    private void PlayVideo()
    {
        video.Play();
        PlayBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
    }

    private void PauseVideo() { 
    video.Pause();
        PlayBtn.gameObject.SetActive(true);
        PauseBtn.gameObject.SetActive(false);
    }


    private string FormatTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60f);
        int seconds = Mathf.FloorToInt((float)time % 60f);
        return string.Format("{0:0}:{1:0}", minutes, seconds);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished!");

        // Optional: reset UI
        PlayBtn.gameObject.SetActive(true);
        PauseBtn.gameObject.SetActive(false);

        // Reset slider
        videoSeekSlider.value = videoSeekSlider.maxValue;
    }
}
