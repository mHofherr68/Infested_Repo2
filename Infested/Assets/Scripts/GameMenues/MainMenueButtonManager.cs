using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenueButtonManager : MonoBehaviour
{
    [Header("Main Navigation Settings")]

    [Space(16)]

    // The relative URL to the main website (must be inside the project's data path).
    [SerializeField] private string websiteURL;

    // The name of the scene that should be loaded when starting the game.
    [SerializeField] private string gameScene;

    // The name of the scene that should be loaded when opening the store.
    [SerializeField] private string storeScene;

    // The name of the scene that should be loaded when opening the options menu.
    [SerializeField] private string optionsScene;

    // The relative URL to the help site (must be inside the project's data path).
    [SerializeField] private string helpsiteURL;

    /// <summary>
    /// Opens the specified website in the default browser, which is located inside the project's data path.
    /// </summary>
    public void WebsitePressed()
    {
        Application.OpenURL("file://" + Application.dataPath + websiteURL);
    }

    /// <summary>
    /// Loads the game scene when the "Play" button is pressed.
    /// </summary>
    public void LoadGamePressed()
    {
        SceneManager.LoadScene(gameScene);
    }

    /// <summary>
    /// Loads the store scene when the "Store" button is pressed.
    /// </summary>
    public void StorePressed()
    {
        SceneManager.LoadScene(storeScene);
    }

    /// <summary>
    /// Loads the options scene when the "Options" button is pressed.
    /// </summary>
    public void OptionsPressed()
    {
        SceneManager.LoadScene(optionsScene);
    }

    /// <summary>
    /// Opens the specified website in the default browser, which is located as a file in a directory.
    /// </summary>
    public void HelpPressed()
    {
        Application.OpenURL("file://" + Application.dataPath + helpsiteURL);
    }

    /// <summary>
    /// Quits the application when the "Quit" button is pressed.
    /// If running inside the Unity Editor, stops play mode instead.
    /// </summary>
    public void QuitPressed()
    {
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
    }
}
