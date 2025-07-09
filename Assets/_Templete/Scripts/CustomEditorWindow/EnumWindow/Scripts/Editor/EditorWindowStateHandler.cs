using UnityEditor;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

[InitializeOnLoad]
public static class EditorWindowStateHelper
{
    public static event Action OnEditorMinimized;
    public static event Action OnEditorRestored;

    private static bool isMinimized = false;

    static EditorWindowStateHelper()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        bool currentlyMinimized = IsEditorMinimized();
        if (currentlyMinimized && !isMinimized)
        {
            isMinimized = true;
            OnEditorMinimized?.Invoke();
        }
        else if (!currentlyMinimized && isMinimized)
        {
            isMinimized = false;
            OnEditorRestored?.Invoke();
        }
    }

    private static bool IsEditorMinimized()
    {
        var mainWindow = GetMainWindowHandle();
        if (mainWindow == IntPtr.Zero)
        {
            return false;
        }

        return IsIconic(mainWindow);
    }

    private static IntPtr GetMainWindowHandle()
    {
        System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        foreach (System.Diagnostics.ProcessThread thread in process.Threads)
        {
            EnumThreadWindows(thread.Id, (hWnd, lParam) =>
            {
                if (GetParent(hWnd) == IntPtr.Zero)
                {
                    return true;
                }
                return false;
            }, IntPtr.Zero);
        }
        return process.MainWindowHandle;
    }

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

    private delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetParent(IntPtr hWnd);
}
