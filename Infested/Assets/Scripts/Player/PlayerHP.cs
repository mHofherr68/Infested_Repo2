using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("UI")]
    [SerializeField] private Image damageScreen; // Ein rotes UI Image, auf 0 Alpha setzen

    [Header("Damage Settings")]
    [SerializeField] private float damageScreenDuration = 0.5f; // Zeit wie lange der Screen sichtbar ist
    private float damageTimer = 0f;

    private void Start()
    {
        currentHP = maxHP;

        // Stellen sicher, dass der rote Screen unsichtbar ist
        if(damageScreen != null)
            damageScreen.color = new Color(1, 0, 0, 0);
    }

    private void Update()
    {
        // Rote Screen-Fade Out
        if(damageScreen != null && damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            float alpha = Mathf.Lerp(0, 0.5f, damageTimer / damageScreenDuration); // halbtransparent
            damageScreen.color = new Color(1, 0, 0, alpha);
        }
    }

    // Wird aufgerufen, wenn ein Projektil den Player trifft
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile")) // Stelle sicher, dass dein Projektil Tag "Projectile" hat
        {
            TakeDamage(20); // Beispiel: 20 Schaden
            Destroy(other.gameObject); // Projektil zerstören
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        // Zeige roten Screen
        if(damageScreen != null)
        {
            damageTimer = damageScreenDuration;
            damageScreen.color = new Color(1, 0, 0, 0.5f);
        }

        if(currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player ist gestorben!");
    }
}