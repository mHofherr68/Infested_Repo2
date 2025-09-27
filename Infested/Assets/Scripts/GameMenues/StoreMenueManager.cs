using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreMenueManager : MonoBehaviour
{
    [Header("Store Navigation Settings")]

    [Space(16)]

    // The name of the scene that should be loaded when the "Back" button is pressed.
    [SerializeField] private string backToScene;

    // The relative URL to the store website (must be located inside the project's data path).
    [SerializeField] private string websiteURL;

    /// <summary>
    /// Opens the specified store website in the system's default web browser.
    /// </summary>
    public void WebsitePressed()
    {
        Application.OpenURL("file://" + Application.dataPath + websiteURL);
    }

    /// <summary>
    /// Loads the selected scene when the "Back" button is pressed.
    /// </summary>
    public void BackPressed()
    {
        SceneManager.LoadScene(backToScene);
    }
}
