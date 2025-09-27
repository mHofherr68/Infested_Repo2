using UnityEngine;

public class SkyBoxRotation : MonoBehaviour
{
    [Header("Skybox Settings")]
    [Space(16)]

    // Set the speed at which the skybox rotates.
    [SerializeField] private float rotateSpeed = 0.2f;

    // Public property for accessing and modifying the rotation speed at runtime.
    public float RotateSpeed
    {
        get => rotateSpeed;
        set => rotateSpeed = value;
    }

    private void Update()
    {
        // Continuously rotate the skybox over time.
        // "_Rotation" is a shader property used by Unity's default skybox material.
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
    }
}
