using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Player Camera Settings")]
    [Space(16)]

    // Reference to the player's orientation (used for movement direction)
    [SerializeField] private Transform orientation; 

    // Mouse sensitivity on the X axis
    [SerializeField] private float mouseMoveSenseX = 40f;  
    
    // Mouse sensitivity on the Y axis
    [SerializeField] private float mouseMoveSenseY = 16f;

    [SerializeField] private float clampUp = -62f;

    [SerializeField] private float clampDown = 45f;

    // Stores the current look input from the mouse
    private Vector2 lookInput;   
    
    // Vertical camera rotation
    private float xRotation; 
    
    // Horizontal camera rotation
    private float yRotation;    
    
    // Reference to the player's input actions
    private PlayerControls controls;  

    private void Awake()
    {

        // Initialize the input controls
        controls = new PlayerControls();

        // Assign mouse movement to look input
        controls.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.LookAround.canceled += ctx => lookInput = Vector2.zero;
    }


    // Enable input controls
    private void OnEnable() => controls.Enable();
    
    // Disable input controls
    private void OnDisable() => controls.Disable(); 

    void Start()
    {

        // Lock and hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial camera direction
        Vector3 direction = Vector3.forward;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;

        // Initialize rotation values
        xRotation = 0f;
        yRotation = 0f;
    }

    private void Update()
    {

        // Apply mouse input scaled by sensitivity and deltaTime
        float mouseX = lookInput.x * Time.deltaTime * mouseMoveSenseX;
        float mouseY = lookInput.y * Time.deltaTime * mouseMoveSenseY;

        // Update vertical and horizontal rotations
        xRotation -= mouseY;
        yRotation += mouseX;

        // Clamp vertical rotation to prevent over-tilting
        xRotation = Mathf.Clamp(xRotation, clampUp, clampDown);

        // Apply rotation to the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Apply horizontal rotation to orientation object (for player movement)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
