using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Head Bobbing Settings")]
    [Space(16)]
    // How quickly the bobbing motion occurs.
    public float bobFrequency = 1.5f;
    // Horizontal intensity of the bobbing motion.
    public float bobHorizontalAmount = 0.05f;
    // Vertical intensity of the bobbing motion.
    public float bobVerticalAmount = 0.05f;

    // Reference to the character controller that provides movement input and grounded state.
    public BaseCharacterController controller;

    // Initial local position of the object (used to reset bobbing).
    private Vector3 startPos;
    // Internal timer used to drive the sine/cosine motion.
    private float timer = 0;

    private void Start()
    {
        // Store the starting local position for reference.
        startPos = transform.localPosition;

        // If no controller is assigned, try to automatically find one in the parent hierarchy.
        if (controller == null)
        {
            controller = GetComponentInParent<BaseCharacterController>();
        }
    }

    private void Update()
    {
        // Determine if the player is currently moving on the ground.
        bool isMoving = controller != null
                      && controller.isGrounded
                      && controller.movementInput.magnitude > 0.1f;

        if (isMoving)
        {
            // Adjust bobbing speed based on current movement speed (including external modifiers).
            float speedFactor = (controller.actualMovementSpeed + controller.externalSpeedOffset) / controller.actualMovementSpeed;

            // Advance the bobbing timer.
            timer += Time.deltaTime * bobFrequency * speedFactor;

            // Calculate bobbing offsets using sine (horizontal) and cosine (vertical).
            float bobX = Mathf.Sin(timer) * bobHorizontalAmount;
            float bobY = Mathf.Cos(timer * 2) * bobVerticalAmount;

            Vector3 horizontalOffset = transform.right * bobX;
            Vector3 verticalOffset = transform.up * bobY;

            // Apply bobbing offsets relative to the starting position.
            transform.localPosition = startPos + horizontalOffset + verticalOffset;
        }
        else
        {
            // Reset timer and smoothly return to the starting position when not moving.
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 5);
        }
    }
}
