using System;
using UnityEngine;

public class DrillRig : MonoBehaviour, IConveyorOutput
{
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] float productionInterval = 1f;
    [SerializeField] Transform outputPoint;
    [SerializeField] OreNode currentOreNode;
    public Transform OutputPoint => outputPoint;

    private float timer;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= productionInterval)
        {
            ProduceItem();
            timer = 0;
        }
    }

    public void SetOutputConveyor(Conveyor conveyor)
    {
        outputConveyor = conveyor;
    }

    public void SetNode(OreNode node) => currentOreNode = node;
  

    private void ProduceItem()
    {
        if (outputConveyor == null)
            return;
        if (!outputConveyor.CanAcceptItem())
            return;
        if (currentOreNode == null)
            return;
        
        ConveyorItem item = Instantiate(
            currentOreNode.OrePrefab, 
            outputConveyor.InputPoint.position, 
            Quaternion.identity);

        outputConveyor.AddItem(item);
    }
}
