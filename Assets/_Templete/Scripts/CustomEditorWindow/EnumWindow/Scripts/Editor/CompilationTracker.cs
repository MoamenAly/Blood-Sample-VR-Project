using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Compilation;

[InitializeOnLoad]
public static class CompilationTracker
{
    public static event Action OncompilationStarted;
    public static event Action OncompilationFinished;
    static CompilationTracker()
    {
        CompilationPipeline.compilationStarted += OnCompilationStarted;
        CompilationPipeline.compilationFinished += OnCompilationFinished;
    }

    private static void OnCompilationStarted(object context)
    {
        Debug.Log("Compilation started");
        // Perform actions before compilation starts
        OncompilationStarted?.Invoke();
    }

    private static void OnCompilationFinished(object context)
    {
        Debug.Log("Compilation finished");
        // Perform actions after compilation finishes
        OncompilationFinished?.Invoke();
    }
}
