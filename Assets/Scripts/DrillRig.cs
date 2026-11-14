using System;
using UnityEngine;

public class DrillRig : MonoBehaviour
{
    [SerializeField] Conveyor outputConveyor;
    [SerializeField] ConveyorItem itemPrefab;
    [SerializeField] float productionInterval = 1f;

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
