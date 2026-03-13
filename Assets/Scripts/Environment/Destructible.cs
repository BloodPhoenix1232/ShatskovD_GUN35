using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float health = 50f;
    public GameObject destroyEffect;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}