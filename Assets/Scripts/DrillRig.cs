using System;
using UnityEngine;

public class DrillRig : MonoBehaviour, IConveyorOutput
{
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] ConveyorItem itemPrefab;
    [SerializeField] float productionInterval = 1f;
    [SerializeField] Transform outputPoint;
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

    private void ProduceItem()
    {
        if (outputConveyor == null)
            return;
        if (!outputConveyor.CanAcceptItem())
            return;
        
        ConveyorItem item = Instantiate(
            itemPrefab, 
            outputConveyor.InputPoint.position, 
            Quaternion.identity);

        outputConveyor.AddItem(item);

    }
}
