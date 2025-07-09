using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f; // Interval to update FPS display
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Time left for current interval

    [SerializeField] TMPro.TextMeshProUGUI m_TextMeshPro;

    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Interval ended, so update FPS
        if (timeleft <= 0.0)
        {
            // Display FPS
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
            m_TextMeshPro.text =  format;
        }
    }

  
}
