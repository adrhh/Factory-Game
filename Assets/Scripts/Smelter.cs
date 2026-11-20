using UnityEngine;

public class Smelter : MonoBehaviour, IConveyorOutput
{
    [SerializeField] Transform outputPoint;
    [SerializeField] Transform inputPoint;
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] ItemData requiredItem;
    [SerializeField] ConveyorItem outputPrefab;
    [SerializeField] float productionTime = 2f;
    public Transform OutputPoint => outputPoint;

    private ConveyorItem storedItem;
    private float timer;

    private void Update()
    {
        if (storedItem == null)
            return;
        
        timer += Time.deltaTime;

        if (timer >= productionTime)
        {
            FinishProduction();
            timer = 0;
        }
    }

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
        
        storedItem = item;
        item.gameObject.SetActive(false);
    }

    public void SetOutputConveyor(Conveyor conveyor)
    {
        outputConveyor = conveyor;
    }

    private void FinishProduction()
    {
        if (outputConveyor == null)
            return;
        if (!outputConveyor.CanAcceptItem())
            return;
        
        ConveyorItem conveyorItem = Instantiate(
            outputPrefab,
            outputPoint.position,
            Quaternion.identity);
        
        outputConveyor.AddItem(conveyorItem);
        Destroy(storedItem.gameObject);
        storedItem = null;
    }
}
