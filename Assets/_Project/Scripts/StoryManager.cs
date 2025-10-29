using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization; // StringInfo
using UnityEngine;
using UnityEngine.Events;

#if TMP_PRESENT
using TMPro;
#endif

// Odin
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if !TMP_PRESENT
using RTLTMPro; // If you're not using TMP
#endif

#region Data Types

public enum TypewriterMode
{
    Off,
    Word,
    Letter
}

[Serializable]
public class StoryStep
{
    [HideInInspector] public bool isCurrent;

    [FoldoutGroup("Step", expanded: true)]
    [HorizontalGroup("Step/Header")]
    [LabelText("ID")]
    public string id;

    [FoldoutGroup("Step")]
    [LabelText("Subtitle")]
    [TextArea(2, 4)]
    [GUIColor("@isCurrent ? new Color(0.82f, 1f, 0.82f) : Color.white")]
    public string subtitle;

    // ---------- Subtitles ----------
    [FoldoutGroup("Step/Subtitles")]
    [LabelText("Typewriter Mode")]
    public TypewriterMode typewriterMode = TypewriterMode.Word;

    [FoldoutGroup("Step/Subtitles")]
    [ShowIf("@typewriterMode != TypewriterMode.Off")]
    [LabelText("Sync To Narration (if available)")]
    public bool syncToNarration = true;

    [FoldoutGroup("Step/Subtitles")]
    [ShowIf("@typewriterMode == TypewriterMode.Word && !syncToNarration")]
    [Min(0.1f), LabelText("Words Per Second")]
    public float wordsPerSecond = 4f;

    [FoldoutGroup("Step/Subtitles")]
    [ShowIf("@typewriterMode == TypewriterMode.Letter && !syncToNarration")]
    [Min(0.1f), LabelText("Letters Per Second")]
    public float lettersPerSecond = 20f;

    // ---------- Audio ----------
    [FoldoutGroup("Step/Audio")]
    [LabelText("Narration")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [GUIColor("@isCurrent ? new Color(0.82f, 1f, 0.82f) : Color.white")]
    public AudioClip narration;

    [FoldoutGroup("Step/Audio")]
    [LabelText("Auto-next on narration end")]
    public bool autoNextOnNarrationEnd = false;

    [FoldoutGroup("Step/Audio")]
    [LabelText("Auto-next Delay (sec)")]
    [Min(0f)]
    public float autoNextDelay = 0f;


    // ---------- Events ----------
    [FoldoutGroup("Step/Events")]
    [LabelText("On Step Start")]
    public UnityEvent2 onStepStart;

    [FoldoutGroup("Step/Events")]
    [LabelText("On Narration Finished")]
    public UnityEvent2 onNarrationFinished;

    // Read-only badge
    [FoldoutGroup("Step")]
    [GUIColor("@isCurrent ? new Color(0.45f, 0.9f, 0.45f) : new Color(0.65f, 0.65f, 0.65f)")]
    private string _currentBadge => isCurrent ? "▶ This Step" : "—";
}

#endregion

public class StoryManager : MonoBehaviour
{
    // ======================
    // Inspector: UI & Audio
    // ======================

    [FoldoutGroup("Settings/UI & Audio", expanded: true)]
#if TMP_PRESENT
    [Required, LabelText("Subtitle (TMP)")]
    public TMP_Text subtitleText;
#else
    [Required, LabelText("Subtitle (UI Text)")]
    public RTLTextMeshPro subtitleText;
#endif

    [FoldoutGroup("Settings/UI & Audio")]
    [Required, LabelText("Audio Source")]
    public AudioSource audioSource;

    // ======================
    // Inspector: Sequence
    // ======================

    [FoldoutGroup("Settings/Sequence", expanded: true)]
    [LabelText("Play On Start")]
    public bool playOnStart = false;

    [FoldoutGroup("Settings/Sequence")]
    [Min(0f), LabelText("Play On Start Delay (sec)")]
    public float playOnStartDelay = 0f;

    [FoldoutGroup("Settings/Sequence")]
    [LabelText("Steps")]
    [ListDrawerSettings(
        Expanded = true,
        DraggableItems = true,
        ShowIndexLabels = true,
        ListElementLabelName = "Step {index}"
    )]
    public List<StoryStep> steps = new List<StoryStep>();

    // ======================
    // Inspector: Global Events
    // ======================

    [FoldoutGroup("Settings/Global Events", expanded: false)]
    [LabelText("On Sequence Started")]
    public UnityEvent2 onSequenceStarted;

    [FoldoutGroup("Settings/Global Events")]
    [LabelText("On Sequence Completed")]
    public UnityEvent2 onSequenceCompleted;

    // ======================
    // Runtime (read-only)
    // ======================

    [InfoBox("@GetCurrentInfoBox()", InfoMessageType = InfoMessageType.Info)]
    [FoldoutGroup("Runtime", expanded: false)]
    [ShowInInspector, ReadOnly, LabelText("Is Running")]
    private bool isSequenceRunning = false;

    [FoldoutGroup("Runtime")]
    [ShowInInspector, ReadOnly, LabelText("Current Index")]
    private int currentIndex = -1;

    [FoldoutGroup("Runtime")]
    [ShowInInspector, ReadOnly, LabelText("Current Step ID")]
    private string CurrentStepId => (currentIndex >= 0 && currentIndex < steps.Count) ? steps[currentIndex].id : "(none)";

    // Internals
    private Coroutine stepRoutine;
    private Coroutine typewriterRoutine;
    private readonly List<Coroutine> timedRoutines = new List<Coroutine>();

    // ======================
    // Life Cycle
    // ======================

    private void Start()
    {
        if (playOnStart)
            StartCoroutine(StartSequenceAfterDelay(playOnStartDelay));
    }

    private IEnumerator StartSequenceAfterDelay(float delaySeconds)
    {
        if (delaySeconds > 0f)
            yield return new WaitForSeconds(delaySeconds);

        StartSequence();
    }

    // ======================
    // Controls
    // ======================

    [FoldoutGroup("Controls", expanded: true)]
    [Button(ButtonSizes.Large)]
    public void StartSequence()
    {
        StopEverything();

        if (steps == null || steps.Count == 0)
        {
            Debug.LogWarning("StoryManager: No steps found.");
            return;
        }

        isSequenceRunning = true;
        onSequenceStarted?.Invoke();
        GoToStep(0);
    }

    [FoldoutGroup("Controls")]
    [EnableIf("@isSequenceRunning")]
    [Button(ButtonSizes.Large)]
    public void NextStep()
    {
        if (!isSequenceRunning) return;
        GoToStep(currentIndex + 1);
    }

    [FoldoutGroup("Controls")]
    [EnableIf("@isSequenceRunning")]
    [Button]
    public void RestartCurrentStep()
    {
        if (currentIndex >= 0 && currentIndex < steps.Count)
            GoToStep(currentIndex);
    }

    [FoldoutGroup("Controls")]
    [Button, GUIColor(1f, 0.5f, 0.5f)]
    public void StopSequence()
    {
        StopEverything();
        isSequenceRunning = false;
        currentIndex = -1;
        SetSubtitle("");
        UpdateCurrentFlags(-1);
    }

    [FoldoutGroup("Controls")]
    [PropertySpace]
    [HorizontalGroup("Controls/Jump", Width = 150)]
    [LabelText("Jump To ID")]
    public string jumpTargetId;

    [HorizontalGroup("Controls/Jump")]
    [Button("Jump", ButtonSizes.Medium)]
    public void JumpToEnteredId()
    {
        if (!string.IsNullOrEmpty(jumpTargetId))
            JumpTo(jumpTargetId);
    }

#if UNITY_EDITOR
    [FoldoutGroup("Controls")]
    [EnableIf("@isSequenceRunning")]
    [Button("Ping Current Step in Inspector")]
    private void PingCurrentStep()
    {
        if (currentIndex >= 0 && currentIndex < steps.Count)
        {
            Selection.activeObject = this;
            EditorGUIUtility.PingObject(this);
        }
    }
#endif

    // Public API
    public void JumpTo(string stepId)
    {
        int idx = steps.FindIndex(s => string.Equals(s.id, stepId, StringComparison.OrdinalIgnoreCase));
        if (idx >= 0) GoToStep(idx);
        else Debug.LogWarning($"StoryManager: No step with ID: {stepId}");
    }

    // ======================
    // Internals
    // ======================

    private void GoToStep(int index)
    {
        if (index >= steps.Count)
        {
            FinishSequence();
            return;
        }

        if (stepRoutine != null) StopCoroutine(stepRoutine);
        StopTimedRoutines();
        StopTypewriter();
        StopAudio();

        currentIndex = index;
        UpdateCurrentFlags(currentIndex);

        var step = steps[currentIndex];
        stepRoutine = StartCoroutine(RunStep(step));
    }

    private IEnumerator RunStep(StoryStep step)
    {
        bool willType = step.typewriterMode != TypewriterMode.Off;
        SetSubtitle(willType ? "" : (step.subtitle ?? ""));

        step.onStepStart?.Invoke();

        if (step.narration != null && audioSource != null)
        {
            audioSource.clip = step.narration;
            audioSource.Play();
        }

        StartTimedEvents(step);

        if (willType)
        {
            StopTypewriter();
            typewriterRoutine = StartCoroutine(Typewriter(
                fullText: step.subtitle ?? "",
                mode: step.typewriterMode,
                syncToNarration: step.syncToNarration,
                narrationClip: step.narration,
                wordsPerSecond: Mathf.Max(0.1f, step.wordsPerSecond),
                lettersPerSecond: Mathf.Max(0.1f, step.lettersPerSecond)
            ));
        }

        if (step.narration != null && audioSource != null)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return null;

            step.onNarrationFinished?.Invoke();

            if (step.autoNextOnNarrationEnd)
            {
                if (step.autoNextDelay > 0f)
                    yield return new WaitForSeconds(step.autoNextDelay);

                NextStep();
            }

        }
    }

    private void StartTimedEvents(StoryStep step)
    {
        StopTimedRoutines();
    }

    private void StopTimedRoutines()
    {
        foreach (var c in timedRoutines)
        {
            if (c != null) StopCoroutine(c);
        }
        timedRoutines.Clear();
    }

    private void StopTypewriter()
    {
        if (typewriterRoutine != null) StopCoroutine(typewriterRoutine);
        typewriterRoutine = null;
    }

    private void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    private void FinishSequence()
    {
        StopEverything();
        onSequenceCompleted?.Invoke();
        UpdateCurrentFlags(-1);
    }

    private void StopEverything()
    {
        if (stepRoutine != null) StopCoroutine(stepRoutine);
        stepRoutine = null;
        StopTimedRoutines();
        StopTypewriter();
        StopAudio();
    }

    private void SetSubtitle(string txt)
    {
        if (subtitleText == null) return;
        subtitleText.text = txt ?? "";
    }

    // ======================
    // Highlight helpers
    // ======================

    private void UpdateCurrentFlags(int activeIndex)
    {
        if (steps == null) return;
        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].isCurrent = (i == activeIndex);
        }

#if UNITY_EDITOR
        if (!Application.isPlaying || EditorApplication.isPlaying)
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }

    private string GetCurrentInfoBox()
    {
        if (!isSequenceRunning) return "Sequence is not running.";
        if (currentIndex < 0 || currentIndex >= steps.Count) return "Sequence running — no current step.";
        var id = string.IsNullOrEmpty(steps[currentIndex].id) ? "(no id)" : steps[currentIndex].id;
        return $"Current Step → Index: {currentIndex}, ID: {id}";
    }

    // ======================
    // Typewriter core
    // ======================

    private IEnumerator Typewriter(
        string fullText,
        TypewriterMode mode,
        bool syncToNarration,
        AudioClip narrationClip,
        float wordsPerSecond,
        float lettersPerSecond)
    {
        if (string.IsNullOrEmpty(fullText))
        {
            SetSubtitle("");
            yield break;
        }

        // Precompute units
        int totalUnits;
        int[] letterStarts = null;  // for Letter mode
        string[] words = null;      // for Word mode

        bool isLetter = (mode == TypewriterMode.Letter);
        if (isLetter)
        {
            // Start indices of each Unicode text element in fullText
            letterStarts = StringInfo.ParseCombiningCharacters(fullText);
            totalUnits = Mathf.Max(1, letterStarts.Length);
        }
        else if (mode == TypewriterMode.Word)
        {
            words = fullText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            totalUnits = Mathf.Max(1, words.Length);
        }
        else
        {
            SetSubtitle(fullText);
            yield break;
        }

        // Can we sync to audio?
        bool canSyncToAudio = syncToNarration &&
                              narrationClip != null &&
                              narrationClip.length > 0.01f &&
                              audioSource != null &&
                              audioSource.clip == narrationClip;

        if (canSyncToAudio)
        {
            float totalDuration = Mathf.Max(0.01f, narrationClip.length);

            while (audioSource != null && audioSource.clip == narrationClip && audioSource.isPlaying)
            {
                float t = Mathf.Clamp(audioSource.time, 0f, totalDuration);
                int show = Mathf.Clamp(Mathf.FloorToInt((t / totalDuration) * totalUnits), 0, totalUnits);

                if (show >= totalUnits)
                {
                    SetSubtitle(fullText);
                    yield break;
                }

                if (isLetter)
                    SetSubtitle(RevealByLetters(fullText, letterStarts, show));
                else
                    SetSubtitle(RevealByWords(fullText, words, show));

                yield return null;
            }

            SetSubtitle(fullText);
            yield break;
        }
        else
        {
            float rate = isLetter ? lettersPerSecond : wordsPerSecond;
            float totalDuration = totalUnits / Mathf.Max(0.1f, rate);
            float elapsed = 0f;

            while (true)
            {
                elapsed += Time.deltaTime;
                int show = Mathf.Clamp(Mathf.FloorToInt((elapsed / totalDuration) * totalUnits), 0, totalUnits);

                if (show >= totalUnits)
                {
                    SetSubtitle(fullText);
                    yield break;
                }

                if (isLetter)
                    SetSubtitle(RevealByLetters(fullText, letterStarts, show));
                else
                    SetSubtitle(RevealByWords(fullText, words, show));

                yield return null;
            }
        }
    }

    /// <summary>
    /// Reveal first 'count' Unicode text elements from fullText using precomputed start indices.
    /// This preserves emojis/diacritics/RTL perfectly and never cuts a grapheme.
    /// </summary>
    private string RevealByLetters(string fullText, int[] letterStarts, int count)
    {
        if (count <= 0) return "";
        if (count >= letterStarts.Length) return fullText;

        int end = (count < letterStarts.Length) ? letterStarts[count] : fullText.Length;
        return fullText.Substring(0, end);
    }

    /// <summary>
    /// Reveal first 'count' words. While typing, words are joined with single spaces;
    /// once complete we snap to the exact original (called by caller when count >= total).
    /// </summary>
    private string RevealByWords(string fullText, string[] words, int count)
    {
        if (count <= 0) return "";
        if (count >= words.Length) return fullText;
        return string.Join(" ", words, 0, count);
    }

    // If your Odin version supports dynamic list element labels:
    // private string GetStepElementLabel(int index) { ... }
}
