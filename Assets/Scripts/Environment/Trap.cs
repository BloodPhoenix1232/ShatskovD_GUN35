using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 1000f;

    [Header("Effects")]
    public ParticleSystem explosionEffect;
    public AudioClip explosionSound;
    public float destroyDelay = 0.1f;

    private bool triggered = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerTrap(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerTrap(other);
    }

    private void TriggerTrap(Collider player)
    {
        if (triggered) return;
        triggered = true;

        PlayerHealth hp = player.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            Vector3 hitDir = Vector3.up;
            hp.TakeDamage(damage, hitDir);
        }

        if (explosionEffect != null)
        {
            ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        Destroy(gameObject, destroyDelay);
    }
}