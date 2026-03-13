using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorTransform;
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = this.transform;

        closedPosition = doorTransform.position;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        Vector3 targetPos = isOpen ? openPosition : closedPosition;
        doorTransform.position = Vector3.Lerp(doorTransform.position, targetPos, Time.deltaTime * openSpeed);
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}