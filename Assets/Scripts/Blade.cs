using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] TrailRenderer _bladeTrail;
    private Vector3 mousePosition;
    [SerializeField] private Grabber _grabber;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateCut();
        }
    }

    private void UpdateCut()
    {
        if (Input.mousePosition != null)
        {
            mousePosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            mousePosition = Input.GetTouch(0).position;
        }

        transform.position = mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.collider.TryGetComponent(out FruitMovement fruit) && _grabber.IsTaken == false)
                {
                    fruit.gameObject.GetComponent<BoxCollider>().enabled = false;
                    fruit.Jump();
                }
            }
        }
    }
}
