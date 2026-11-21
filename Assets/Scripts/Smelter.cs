using Unity.VisualScripting;
using UnityEngine;

public class Smelter : MonoBehaviour, IConveyorOutput
{
    [SerializeField] Transform outputPoint;
    [SerializeField] Transform inputPoint;
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] ItemData requiredItem;
    [SerializeField] ConveyorItem outputPrefab;
    [SerializeField] float productionTime = 2f;
    [SerializeField] int maxOre = 10;
    [SerializeField] int orePerIngot = 2; 
    public Transform OutputPoint => outputPoint;

    private int storedOre;

    private float timer;

    private void Update()
    {
        if (storedOre < orePerIngot)
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
        Debug.Log("OnTriggerEnter! start");
        ConveyorItem item = other.GetComponent<ConveyorItem>();
        if (item == null)
        {
            Debug.Log("item == null");
            return;
        }

        if (item.ItemData != requiredItem)
        {
            Debug.Log(item + " != " + requiredItem);
            return;
        }
        if (storedOre > maxOre)
            return;


        Conveyor conveyor = item.currentConveyor;
        Debug.Log(conveyor);
        if (conveyor != null)
        {
            Debug.Log("conveyor != null");
            conveyor.TakeItem(item);
        }
        
        storedOre++;
        item.gameObject.SetActive(false);
        Destroy(item.gameObject);
        Debug.Log("OnTriggerEnter! end");
    }

    public void SetOutputConveyor(Conveyor conveyor)
    {
        outputConveyor = conveyor;
    }

    private void FinishProduction()
    {
        Debug.Log("FinishProduction! start");
        if (outputConveyor == null)
            return;
        if (!outputConveyor.CanAcceptItem())
            return;

        storedOre -= orePerIngot;
        
        ConveyorItem conveyorItem = Instantiate(
            outputPrefab,
            outputPoint.position,
            Quaternion.identity);
        
        outputConveyor.AddItem(conveyorItem);
    }
}
