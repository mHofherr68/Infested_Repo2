using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class BaseCharacterController : MonoBehaviour
{
    // Set externally by "PlayAudioTrigger.cs" to temporarily modify the player's movement speed.
    [HideInInspector] public float externalSpeedOffset = 0f;

    // Set externally by "PlayAudioTrigger.cs" to lock player input (e.g., during cutscenes or interactions).
    [HideInInspector] public bool inputLocked = false;

    private PlayerInput playerInput;
    private Rigidbody rb;

    // Current movement input vector (X = horizontal, Y = vertical).
    [HideInInspector] public Vector2 movementInput;

    // Reference to the main camera's transform for movement direction.
    private Transform cameraTransform;

    [Header("Player Movement Settings")]
    [Space(16)]
    // Base player movement speed.
    public float actualMovementSpeed = 8f;
    // Force applied when jumping.
    [SerializeField] private float jumpStrength = 8f;
    // Maximum vertical jump velocity.
    [SerializeField] private float maxJumpVelocity = 10f;

    [Header("UI Settings -> Inventory")]
    [Space(16)]
    // Reference to the inventory panel UI.
    [SerializeField] private GameObject selectPanel;


    [Header("Player Ground Detection")]
    [Space(16)]
    // True if the player is standing on the ground.
    public bool isGrounded;
    // Distance for the ground detection ray.
    [SerializeField] private float raycastDistance;
    // Which layers are considered as ground.
    [SerializeField] private LayerMask groundLayer;

    [Header("UI Settings")]
    [Space(16)]
    // Reference to the HealthBar script for managing player health
    [SerializeField] private HealthBarManager healthBarManager;
    // Recoil force applied to the player when taking damage
    [SerializeField] private float recoilForce = 7f;
    // Flag to prevent multiple damage instances from a single collision
    private bool hasTakenDamage = false;

    //[SerializeField] private OxygenManager oxygenManager;
    //[SerializeField] private AmmoManager ammomanager;

    [Header("Collision Stop Settings")]
    [Space(16)]
    // Distance to check for walls in front of the player.
    [SerializeField] private float collisionCheckDistance = 0.5f;
    // Which layers are considered as walls.
    [SerializeField] private LayerMask wallLayer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        //healthBarManager = GameObject.Find("UI_Canvas/UI/HelmetUIElements/HealthBarCircular").GetComponent<HealthBarManager>();

        // Subscribe to input action callbacks
        playerInput.actions["Move"].performed += onMove;
        playerInput.actions["Move"].canceled += onMove;
        playerInput.actions["Jump"].performed += onJump;
        playerInput.actions["Jump"].canceled += onJump;
        playerInput.actions["Inventory"].performed += onInventory;

        cameraTransform = Camera.main.transform;
    }

    /// <summary>
    /// Handles player movement input.
    /// </summary>
    public void onMove(CallbackContext ctx)
    {
        if (inputLocked)
        {
            movementInput = Vector2.zero;
            return;
        }
        movementInput = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles jump input. Adds upward force if grounded.
    /// </summary>
    public void onJump(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (inputLocked) return;

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        
            // Clamp jump velocity to avoid excessive upward force
            Vector3 vel = rb.linearVelocity;
            if (vel.y > maxJumpVelocity)
            {
                vel.y = maxJumpVelocity;
                rb.linearVelocity = vel;
            }
        }
    }

    // Method to determine what happens when player enters collision with another object
    private void OnCollisionEnter(Collision collision)
    {   // Check if the player collides with an enemy
        if (collision.gameObject.CompareTag("Enemy") && !hasTakenDamage)
        {
            Debug.Log("Player Hit by Enemy");
            // Call the TakeDamage method from the HealthManager script
            healthBarManager.RemoveHealth(healthBarManager.damage);
            // Set the flag to true to prevent multiple damage instances
            hasTakenDamage = true;

            // Variable holding the direction of recoil 
            Vector3 recoilDirection = (transform.position - collision.transform.position).normalized;

            // Apply recoil to the Player GameObject
            rb.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);
        }
    }

    // Method to determine what happens when player exits collision with another object
    private void OnCollisionExit(Collision collision)
    {   // Reset the damage flag when the player exits collision with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasTakenDamage = false;
        }
    }

    /// <summary>
    /// Handles inventory input. Toggles the inventory UI panel.
    /// </summary>
    public void onInventory(CallbackContext ctx)
    {
        if (ctx.performed && selectPanel != null)
        {
            selectPanel.SetActive(!selectPanel.activeSelf);
        }
    }

    /// <summary>
    /// Casts a ray downward to check if the player is grounded.
    /// </summary>
    private bool RaycastHitsGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        return isGrounded;
    }

    /// <summary>
    /// Refreshes movement input manually (used when input may need to be re-polled).
    /// </summary>
    public void RefreshInput()
    {
        if (inputLocked || playerInput == null) return;
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (inputLocked) movementInput = Vector2.zero;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        // Movement is relative to camera orientation
        Vector3 movementDirection = (cameraTransform.right * movementInput.x) + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up);

        // Check for walls in movement direction
        if (movementInput.magnitude > 0.01f)
        {
            float radius = 0.3f;
            float height = 2f;
            Vector3 point1 = transform.position + Vector3.up * radius;
            Vector3 point2 = transform.position + Vector3.up * (height - radius);

            if (Physics.CapsuleCast(point1, point2, radius, movementDirection.normalized, out RaycastHit hit, collisionCheckDistance, wallLayer))
            {
                Debug.Log("Wall hit: " + hit.collider.name);
                movementDirection = Vector3.zero;
                movementInput = Vector2.zero;
            }
        }

        // Apply movement
        transform.Translate(movementDirection * Time.deltaTime * (actualMovementSpeed + externalSpeedOffset));

        // Update grounded state
        RaycastHitsGround();
    }
}
