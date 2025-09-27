using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Spieler treffen, Schaden etc.
            Destroy(gameObject); // Projektil zerstören
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            // Projektil bei Wänden/Ground zerstören
            Destroy(gameObject);
        }
    }
}
