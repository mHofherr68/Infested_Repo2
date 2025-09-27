using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBarManager : MonoBehaviour
{
    // Maximum oxygen value
    public float maxOxygen= 100; 

    public float currentOxygen; // Current oxygen value

    [Header("Auto Depletion Settings")]
    public bool enableAutoDepletion = true; // Toggle for enabling/disabling O2 depletion
    public float depletionInterval = 15f; // Interval of O2 depletion in seconds
    public float depletionPercentage = 1f; // Percentage of depletion per interval


    // [SerializeField] private Slider slider;
    // public Gradient gradient;

    // Reference to the UI Image component representing the oxygen bar filling
    [SerializeField] private Image oxygenBarFilling;
    // Reference to the ongoing depletion coroutine
    private Coroutine depletionCoroutine; 

    void Start()
    {   // Initialize oxygen bar filling and current oxygen
        oxygenBarFilling = GameObject.Find("OxygenBarCircular/Fill").GetComponent<Image>();
        // Set current oxygen to maximum at start
        currentOxygen = maxOxygen;

        // Start automatic oxygen depletion if enabled
        if (enableAutoDepletion) 
        {
            StartOxygenDepletion();
        }
    }

    /*
    public void SetMaxOxygen(int oxygen) 
    {
        slider.maxValue = oxygen;
        slider.value = oxygen;

        // oxygenBarFilling.color = gradient.Evaluate(1f); 
    }

    public void SetOxygen(int oxygen) 
    {
        slider.value = oxygen;

        // oxygenBarFilling.color = gradient.Evaluate(slider.normalizedValue);
    }
    */

    // Method to add oxygen to the player
    public void AddOxygen(float oxygen)
    {
        // Increase current oxygen by the specified amount
        currentOxygen += oxygen;
        // Ensure current oxygen does not exceed maximum oxygen
        if (currentOxygen > maxOxygen)
            currentOxygen = maxOxygen;
        // Update the oxygen bar UI to reflect the new oxygen level
        UpdateOxygen();
    }

    // Method to remove oxygen from the player
    public bool RemoveOxygen(float oxygen)
    {
        // Decrease current oxygen by the specified amount
        currentOxygen -= oxygen;
        // Check if current oxygen has dropped to zero or below
        if (currentOxygen <= 0)
        {
            // Set current oxygen to zero and log "Game Over"
            currentOxygen = 0;
            // Update the oxygen bar UI
            UpdateOxygen();
            // Return value indicating oxygen has been depleted
            return true;

        }
        UpdateOxygen();
        // Return value indicating oxygen is still available
        return false;
    }

    // Method to update the oxygen bar UI based on current oxygen
    private void UpdateOxygen()
    {
        // Update the fill amount of the oxygen bar image
        oxygenBarFilling.fillAmount = (currentOxygen / maxOxygen);
    }

    // Coroutine for automatic oxygen depletion
    private IEnumerator OxygenDepletionCoroutine()
    {
        // Continue depleting oxygen at set intervals while enabled and oxygen is available
        while (enableAutoDepletion && currentOxygen > 0)
        {
            // Wait for the specified depletion interval
            yield return new WaitForSeconds(depletionInterval);

            // As long as auto depletion is enabled and there is oxygen left, deplete oxygen
            if (enableAutoDepletion && currentOxygen > 0)
            {
                // Calculate the amount of oxygen to remove based on the depletion percentage
                float oxygenToRemove = maxOxygen * (depletionPercentage / 100f);
                // Remove the calculated amount of oxygen
                RemoveOxygen(oxygenToRemove);
            }
        }
    }

    // ----- Public methods to control auto depletion ----- //

    // Method to start automatic oxygen depletion
    public void StartOxygenDepletion()
    {
        // Start the depletion coroutine if not already running and there is oxygen available
        if (depletionCoroutine == null && currentOxygen > 0)
        {
            // Enable auto depletion and start the coroutine
            enableAutoDepletion = true;
            // Start the coroutine and keep a reference to it
            depletionCoroutine = StartCoroutine(OxygenDepletionCoroutine());
        }
    }

    // Method to stop automatic oxygen depletion
    public void StopOxygenDepletion()
    {
        // Disable auto depletion and stop the coroutine if it is running
        enableAutoDepletion = false;
        // Stop the coroutine if it is active
        if (depletionCoroutine != null)
        {   // Stop the coroutine 
            StopCoroutine(depletionCoroutine);
            // Clear the reference to indicate the coroutine is no longer running
            depletionCoroutine = null;
        }
    }

    // Method to pause automatic oxygen depletion without stopping the coroutine
    public void PauseOxygenDepletion()
    {
        // Simply disable auto depletion; the coroutine will check this flag
        enableAutoDepletion = false;
    }

    // Method to resume automatic oxygen depletion if there is oxygen available
    public void ResumeOxygenDepletion()
    {
        // Re-enable auto depletion only if there is oxygen left
        if (currentOxygen > 0)
        {
            // Set the flag to enable auto depletion
            enableAutoDepletion = true;
            // If the coroutine is not running, start it
            if (depletionCoroutine == null)
            {
                // Start the depletion coroutine
                StartOxygenDepletion();
            }
        }
    }

    // Ensure depletion stops when the object is disabled
    void OnDisable()
    {
        StopOxygenDepletion();
    }

}
