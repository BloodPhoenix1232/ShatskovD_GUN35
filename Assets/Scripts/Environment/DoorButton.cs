using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Door linkedDoor;
    public bool autoClose = true;
    public float closeDelay = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            linkedDoor.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && autoClose)
        {
            if (linkedDoor != null)
                linkedDoor.Invoke(nameof(Door.CloseDoor), closeDelay);
        }
    }
}