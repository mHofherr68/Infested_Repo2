using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [Header("Move Camera Settings")]
    [Space(16)]

    // Reference to the target camera position
    public Transform camPos;

    private void Update()
    {
        // Set this object's position to match the target camera position every frame
        transform.position = camPos.position;
    }
}
