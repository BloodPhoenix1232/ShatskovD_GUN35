using UnityEngine;
using System.Collections;


public class Pin : MonoBehaviour
{
    private bool isDown = false;

    void Update()
    {
        if (!isDown && Vector3.Angle(transform.up, Vector3.up) > 30f)
        {
            isDown = true;

            GameManager.Instance.PinDown();
            StartCoroutine(DestroyWithDelay());
        }
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
