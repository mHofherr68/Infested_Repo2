using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    [Header("Loading Scene Settings")]
    [Space(16)]

    // Reference to the VideoPlayer component that plays the loading video.
    public VideoPlayer videoPlayer;

    // The name of the main game scene to load asynchronously in the background.
    [SerializeField] private string gameScene;

    // Holds the asynchronous loading operation.
    private AsyncOperation asyncLoad;

    private void Start()
    {
        // Start loading the target scene in the background.
        StartCoroutine(LoadLevelAsync(gameScene));

        // Subscribe to the event triggered when the video finishes playing.
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    /// <summary>
    /// Loads a scene asynchronously without immediately activating it.
    /// </summary>
    private IEnumerator LoadLevelAsync(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating until explicitly allowed.
        asyncLoad.allowSceneActivation = false;

        // Yield until the operation is complete (but not activated).
        yield return asyncLoad;
    }

    /// <summary>
    /// Called when the video finishes playing. 
    /// Activates the scene once loading has finished.
    /// </summary>
    private void OnVideoFinished(VideoPlayer vp)
    {
        if (asyncLoad != null)
            asyncLoad.allowSceneActivation = true;
    }
}
