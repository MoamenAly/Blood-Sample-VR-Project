using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// manage all way to load scene
/// we can add new method later for example async load scene
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] ScreenFader screenFader;

    public void LoadSceneWithFade(string sceneName)
    {
        screenFader.DoFadeIn();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        LoadSceneWithFade(sceneName);
    }

    public void Exit() { 
     Application.Quit();
    }
}
