using UnityEngine;

public class Smelter : MonoBehaviour
{
    [SerializeField] Transform outputPoint;
    [SerializeField] Transform inputPoint;
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] ItemData requiredItem;
    [SerializeField] ConveyorItem outputPrefab;
    [SerializeField] float productionTime = 2f;

    private ConveyorItem storedItem;
    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter!");
        ConveyorItem item = other.GetComponent<ConveyorItem>();
        if (item == null)
            return;

        if (item != requiredItem)
            return;

        Conveyor conveyor = item.currentConveyor;
        if (conveyor != null)
            conveyor.TakeItem(item);
        
        Destroy(item.gameObject);
        timer = 0;
    }
}
