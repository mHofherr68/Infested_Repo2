using UnityEngine;

public class Spray : MonoBehaviour
{
    public float Radius = 5f;       // Wirkungsbereich
    public float Distance = 10f;    // Max Reichweite Spray
    public float Damage = 120f;      // Schaden pro Tick
    private bool IsSpraying = false;

    RaycastHit hit;
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Spray.performed += ctx => IsSpraying = true;
        playerControls.Player.Spray.canceled  += ctx => IsSpraying = false;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        if (!IsSpraying) return;
        if (Camera.main == null) return;

        // Mittelpunkt bestimmen (Raycast vom Fadenkreuz)
        if (!Physics.Raycast(
            Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)),
            out hit, Distance)) return;

        // Alle Collider im Umkreis treffen
        Collider[] hits = Physics.OverlapSphere(hit.point, Radius);
        foreach (Collider col in hits)
        {
            EnemyHealth enemy = col.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmo nur zur Visualisierung im Editor
        if (hit.point != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, Radius);
        }
    }
}
