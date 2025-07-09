using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Linq;


public class AudioManager : MonoBehaviour
{
    public float loopInterval = 15;

    public AudioLocoliztion locoliztion;

    [ListDrawerSettings(ShowPaging = false)] [Space]public List<AduioNarration> audioNarrations;

    public void _UpdateAudioLocaliztion(AudioLocoliztion locoliztion) { 
     this.locoliztion = locoliztion;
        Fetch();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Fetch();
    }

    

#endif

    [Button]
    public void Fetch()
    {
        int numOfIds = Enum.GetValues(typeof(Narrations)).Length;
        AduioNarration[] temp = new AduioNarration[numOfIds];
        for (int i = 0; i < temp.Length; i++)
        {
            if (audioNarrations.Count > i && audioNarrations[i] != null)
            {
                audioNarrations[i].UpdateAudio(locoliztion.audioNarrations[i]);
                temp[i] = audioNarrations[i];   
                
            }
            else
            {
                temp[i] = new AduioNarration
                {
                    narrationsId = (Narrations)i,
                     

                };

                temp[i].UpdateAudio(locoliztion.audioNarrations[i]);

            }
        }
        audioNarrations = temp.ToList();
    }

    private AudioSource audioSource;
    private int currentPlayIndex = -1; 
    private AduioNarration currentPlayNarration ; 
    private Coroutine currentPlayCoroutine = null;
    private static AudioManager instance;   

    public static AudioManager Instance
    {
        get { return instance; }
    }  


    private void Awake()
    {
        
            instance = this;
       
    }
    
    void Start()
    {
       // LanguageManager.Instance.OnLangugaesUpdated += OnLanguageUpdated;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Fetch();
    }

    private void OnLanguageUpdated(LanguageLayout layout)
    {
        Fetch();
    }

    public void Play(int index)
    {
        if (index >= 0 && index < audioNarrations.Count)
        {
            if (currentPlayIndex != index)
            {
                currentPlayIndex = index;

                if (currentPlayCoroutine != null)
                {
                    StopCoroutine(currentPlayCoroutine);
                    audioSource.Stop(); 
                }
                currentPlayCoroutine = StartCoroutine(PlaySequentially(index));
            }
        }
        else
        {
            Debug.LogError("Play: Index out of range.");
        }
    }

    public void Play(Narrations narrationId)
    {
        AduioNarration aduioNarration = EnumUtilits<AduioNarration>.Find(audioNarrations, (int)narrationId);

        if (!ReferenceEquals(aduioNarration , null))
        {
            if (currentPlayNarration != aduioNarration || !aduioNarration.isLoop)
            {
                currentPlayNarration = aduioNarration;

                if (currentPlayCoroutine != null)
                {
                    StopCoroutine(currentPlayCoroutine);
                    audioSource.Stop();
                }

                currentPlayCoroutine = StartCoroutine(PlaySequentially(aduioNarration));
            }
        }
        else
        {
            Debug.LogError("Play: Index out of range.");
        }
    }

    
    IEnumerator PlaySequentially(int narationIndex)
    { 
        do
        {
            foreach (AudioClip clip in audioNarrations[narationIndex].audioClips)
            {
                if (narationIndex != currentPlayIndex)
                {
                    break; // Exit if a new request is made
                }
                if (clip != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    yield return new WaitForSeconds(clip.length);
                }
            }

            audioNarrations[narationIndex].onFinished?.Invoke();

            if (audioNarrations[narationIndex].isLoop)
            {
                yield return new WaitForSeconds(loopInterval);
            }
            else
                break;
        }
        while (true);

        if (narationIndex == currentPlayIndex)
        {
            currentPlayCoroutine = null;
        }
    }
    
    IEnumerator PlaySequentially(AduioNarration aduioNarration)
    { 
        do
        {
            foreach (AudioClip clip in aduioNarration.audioClips)
            {
                if (aduioNarration != currentPlayNarration)
                {
                    break; // Exit if a new request is made
                }

                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
            }

            aduioNarration.onFinished?.Invoke();

            if (aduioNarration.isLoop)
            {
                yield return new WaitForSeconds(loopInterval);
            }
            else
                break;
        }
        while (true);

        if (aduioNarration == currentPlayNarration)
        {
            currentPlayCoroutine = null;
        }
    }

    internal void PlaySoundEffect(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}


[Serializable]
public class AduioNarration : EnumComparable
{
    [FoldoutGroup("$AudioName")]
    public Narrations narrationsId;
    [FoldoutGroup("$AudioName")]

    [Space]
    [GUIColor(0.55f, 0.95f, 0.55f)]
    [TextArea(3, 10)]
    public string Description;
    [FoldoutGroup("$AudioName")]
    public bool isLoop = true;
    [FoldoutGroup("$AudioName")]
    [Space] internal List<AudioClip> audioClips;
    [FoldoutGroup("$AudioName")]

    [Space(3)]
    [GUIColor(1.0f, 0.9373f, 0.8353f)]
    [Space] public UnityEvent2 onFinished;
    private string AudioName => narrationsId.ToString();
    public bool Compare(int id)
    {
        if ((int)narrationsId == id) return true;
        else return false;
    }

    internal AudioData ToAudio()
    {
        return new AudioData()
        {
            audioClips = this.audioClips,
            narrationsId = this.narrationsId
        };
    }

    internal void UpdateAudio(AudioData audioData)
    {
        this.audioClips   = audioData.audioClips;
        this.narrationsId = audioData.narrationsId;
    }
}


