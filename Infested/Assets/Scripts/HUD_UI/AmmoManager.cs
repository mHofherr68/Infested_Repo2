using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// This code is adapted from the book "Unity UI Cookbook" from Francesco Sapio,
// Packt Publishing Ltd, 2015, pages 28-29
public class AmmoManager : MonoBehaviour
{
    // Reference to the TextMeshPro text component displaying the ammo count
    [SerializeField] private TMP_Text ammoDisplayText;
    // Current ammo count
    public int currentAmmo = 10; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   // Initialize current ammo and find the ammo display text component in the scene
        ammoDisplayText = GameObject.Find("AmmoDisplay/AmmoDisplayText").GetComponent<TMP_Text>();
        // Set an initial ammo count
        UpdateAmmoCounter(); 
    }

    // Method to add ammo to the player
    public void AddAmmo(int ammo)
    {
        // Increase current ammo by the specified amount
        currentAmmo += ammo;
        // Update the ammo display text to reflect the new ammo count
        UpdateAmmoCounter();

    }

    // Method to reduce ammo when the player uses a weapon
    public void ReduceAmmo(int ammo)
    {
        // Decrease current ammo by the specified amount
        currentAmmo -= ammo;
        // Ensure current ammo does not drop below zero
        UpdateAmmoCounter();
    }

    // Method to update the ammo display text
    private void UpdateAmmoCounter()
    {   // Update the ammo display text with the current ammo count
        ammoDisplayText.text = currentAmmo.ToString();
    }
}
