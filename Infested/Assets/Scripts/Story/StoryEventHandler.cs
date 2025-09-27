using UnityEngine;
using System.Collections;

public class StoryEventHandler : MonoBehaviour
{
    [Header("Audioclip Settings")]
    [Space(16)]

    // Selected Audioclip will be played when triggered at Boxcollider -> trigger
    [SerializeField] private AudioClip audioClip;

    // When set "true", audio will only play once
    [SerializeField] private bool playOnlyOnce = true;

    [Header("Player Movement Settings")]
    [Space(16)]

    // Player movement adjustment (+/-), e.g. to slow down or create a "jump back" effect during audio
    [SerializeField] private float slowDuringAudio = 0f;

    // When set "true", player movement and head-bobbing will stop while the audio is playing
    [SerializeField] private bool stopDuringAudio = false;

    [Header("Optional UI / Object Settings after Audio")]
    [Space(16)]

    // Optional subtitles, or in the story context e.g. a "Helmet UI" that spawns after the audio
    [SerializeField] private GameObject selectPanel;

    // Destroy the selected story "GameObject" at the end
    [SerializeField] private GameObject destroyTarget;

    // Next story "GameObject" that will be activated after audio completes
    [SerializeField] private GameObject nextTarged;

    // Private variables used internally in this script
    private AudioSource audioSource;
    private bool hasPlayed = false;
    private BaseCharacterController currentController;

    void Start()
    {
        // Add and configure an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;

        // Ensure optional panel is hidden initially
        if (selectPanel != null)
        {
            // Preset value
            selectPanel.SetActive(false);
        }

        // Ensure the next story object is initially deactivated
        if (nextTarged != null)
        {
            // Preset value
            nextTarged.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Only react when the object entering the trigger has the "Player" tag
        if (!other.CompareTag("Player")) return;

        // Attempt to get the player's character controller
        BaseCharacterController controller = other.GetComponent<BaseCharacterController>();
        if (controller == null)
        {
            controller = other.GetComponentInParent<BaseCharacterController>();
        }

        // If a controller was found, store it for use during audio playback
        if (controller != null)
        {
            currentController = controller;
        }

        // Skip playback if audio should only play once and has already been played
        if (playOnlyOnce && hasPlayed) return;

        // Prevent multiple overlapping playbacks if replay is allowed
        if (!playOnlyOnce && audioSource.isPlaying) return;

        // Start audio playback if an audio clip is assigned
        if (audioClip != null)
        {
            StartCoroutine(PlayAudioAndHandleEvents());
        }
    }

    private IEnumerator PlayAudioAndHandleEvents()
    {
        hasPlayed = true;
        audioSource.Play();

        // Lock player input if configured to stop during audio (HeadBanging is also stopped)
        if (stopDuringAudio && currentController != null)
        {
            currentController.inputLocked = true;
            currentController.RefreshInput();
        }

        // While audio is playing, optionally change Player movement -+
        while (audioSource.isPlaying)
        {
            if (currentController != null)
            {
                currentController.externalSpeedOffset = slowDuringAudio;
            }
            yield return null;
        }

        // Once audio has finished, reset player state
        if (currentController != null)
        {
            // Reset any temporary speed adjustments applied during audio playback
            currentController.externalSpeedOffset = 0f;

            // If player input was locked during audio, unlock it and refresh input handling
            if (stopDuringAudio)
            {
                currentController.inputLocked = false;
                currentController.RefreshInput();
            }

            // Clear the reference to the current controller to avoid unintended interactions
            currentController = null;
        }

        // Optional: enable UI panel
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }

        // Optional: destroy a target object
        if (destroyTarget != null)
        {
            Destroy(destroyTarget);
        }

        // Optional: activate the next story object
        if (nextTarged != null)
        {
            nextTarged.SetActive(true);
        }
    }
}
