using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This code is adapted from the book "Unity UI Cookbook" from Francesco Sapio,
// Packt Publishing Ltd, 2015, pages 46-51
public class HealthBarManager : MonoBehaviour
{
    // Reference to the UI Image component representing the health bar filling
    [SerializeField] private Image healthbarFilling;

    [SerializeField] private Slider slider;

    // ----- Circular movement properties ----- //
    // Center point of the circular path
    [SerializeField] private RectTransform circularCenter;
    // Radius of the circular path
    [SerializeField] private float circularRadius = 180f;
    // Starting angle (90° = top, 0° = right)
    [SerializeField] private float startAngle = 100f;
    // Total angle range 360° 
    [SerializeField] private float totalAngle = 360f;      



    // Maximum health value
    public float maxHealth = 100f;
    // Current health value
    public float currentHealth = 20f;
    // Damage value from enemy collision
    public float damage = 10f;



    void Start()
    {   // Initialize health bar filling and current health
        healthbarFilling = GameObject.Find("HealthBarCircular/Fill").GetComponent<Image>();

        // If no circular center is assigned, use the slider's rect transform as center
        if (circularCenter == null)
            circularCenter = slider.GetComponent<RectTransform>();

        // IMPORTANT: Set the slider's max value first!
        slider.maxValue = maxHealth;

        // Set current health to 20 at the start
        SetHealth((int)currentHealth);

        // Update the visual components
        UpdateHealth();
    }


    void Update()
    {
        // Update handle position every frame (you could optimize this by only calling when health changes)
        UpdateCircularHandle();
    }

    // Method to set the current health on the slider
    public void SetHealth(int health)
    {
        // Set the slider's value to the current health
        slider.value = health;
    }
    
    /*
    public void SetMaxHealth(int health)
    {
        // Set the slider's maximum value and current value to the specified health
        slider.maxValue = health;
        // Initialize the slider's value to the maximum health
        slider.value = health;

    } */

    // Method to add health to the player
    public void AddHealth(float heal) {
        // Increase current health by the specified heal amount
        currentHealth += heal;
        // Ensure current health does not exceed maximum health
        if (currentHealth> maxHealth)
            currentHealth = maxHealth;
        // Update the health bar UI
        UpdateHealth();
    }

    // Method to remove health from the player
    public bool RemoveHealth(float damage)
    {
        // Decrease current health by the specified damage amount
        currentHealth -= damage;
        // Check if current health has dropped to zero or below
        if (currentHealth <= 0)
        {
            // Set current health to zero and log "Game Over"
            currentHealth = 0;
            // Update the health bar UI
            UpdateHealth();
            // Return true to indicate the player has died
            return true;
            
        }
        // Update the health bar UI
        UpdateHealth();
        // Return false to indicate the player is still alive
        return false;
    }

    // Method to update the health bar UI based on current health
    private void UpdateHealth()
    {
        // Update the fill amount of the health bar image; works for normal bar and circular bar
        healthbarFilling.fillAmount = (currentHealth / maxHealth);

        // Update slider value and handle position
        slider.value = currentHealth;
        UpdateCircularHandle();
    }

    // Method to update the handle position along a circular path based on current health
    private void UpdateCircularHandle()
    {
        // Ensure the handleRect and circularCenter are assigned
        if (slider.handleRect != null && circularCenter != null)
        {
            // Calculate the normalized position (0 to 1) based on slider value
            float normalizedValue = currentHealth / maxHealth;

            // Calculate the angle based on the normalized value
            float currentAngle = startAngle - (normalizedValue * totalAngle);

            // Convert angle to radians
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            // Calculate position on the circle
            Vector2 circularPosition = new Vector2(
                Mathf.Cos(angleInRadians) * circularRadius,
                Mathf.Sin(angleInRadians) * circularRadius
            );

            // Set the handle position relative to the circular center
            slider.handleRect.anchoredPosition = circularPosition;
        }
    }

}
