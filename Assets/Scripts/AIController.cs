using UnityEngine;

public class AIController : MonoBehaviour
{
    public float searchRadius = 5f;
    private GameObject targetItem;

    [HideInInspector] public bool canSearch = false;

    public void SetTarget(GameObject item) { targetItem = item; }
    public GameObject GetTargetItem() { return targetItem; }
    public void ClearTarget() { targetItem = null; }
}
