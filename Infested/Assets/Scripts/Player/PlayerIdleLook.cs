using UnityEngine;


// OPTIONAL


/*public class PlayerIdleLook : MonoBehaviour
{
    public Transform cameraTransform;
    public float idleTimeBeforeLook = 5f;
    public float lookSpeed = 0.5f;
    public float lookAngle = 10f;

    private float idleTimer = 0f;
    private bool isLookingAround = false;
    private Vector3 initialRotation;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        initialRotation = cameraTransform.localEulerAngles;
    }

    void Update()
    {
        // Reset Timer wenn Input da ist
        if (Input.anyKey || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
        {
            idleTimer = 0f;
            isLookingAround = false;
            cameraTransform.localEulerAngles = initialRotation;
            return;
        }

        // Timer hochzählen
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTimeBeforeLook)
        {
            isLookingAround = true;
        }

        // Automatisches Umschauen
        if (isLookingAround)
        {
            float yaw = Mathf.Sin(Time.time * lookSpeed) * lookAngle;
            float pitch = Mathf.Sin(Time.time * lookSpeed * 0.5f) * (lookAngle / 2);

            cameraTransform.localEulerAngles = initialRotation + new Vector3(pitch, yaw, 0);
        }
    }
}*/