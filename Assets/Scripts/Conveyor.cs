using System;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] Transform inputPoint;
    [SerializeField] Transform outputPoint;
    public List<ConveyorItem> Items = new ();
    [SerializeField] float moveSpeed = 2f;

    public Transform InputPoint => inputPoint;
    public Transform OutputPoint => outputPoint;
    public Conveyor nextConveyor;
    public Conveyor prevConveyor;

    // TEST
    [SerializeField] ConveyorItem test;

    private void SpawnTestItem()
    {
        if (Input.GetKeyDown(KeyCode.T) && test != null)
        {
            ConveyorItem item = Instantiate(test, InputPoint.position, Quaternion.identity);
            item.moveProgress = 0;
            Items.Add(item);
        }
    }
    private void Update()
    {
        MoveItems();
        SpawnTestItem();
    }
    private void MoveItems()
    {
        for (int i = Items.Count - 1; i > 0; i--)
        {
            ConveyorItem conveyorItem = Items[i];
            conveyorItem.moveProgress += moveSpeed * Time.deltaTime;
            conveyorItem.moveProgress = Mathf.Clamp01(conveyorItem.moveProgress);
            conveyorItem.transform.position = Vector3.Lerp(
                InputPoint.position, 
                OutputPoint.position,
                conveyorItem.moveProgress);
            if (conveyorItem.moveProgress >= 1)
            {
                TransferItem(conveyorItem);
            }
        }
    }

    private void TransferItem(ConveyorItem conveyorItem)
    {
        string nextConveyorName = conveyorItem.ToString() ?? "null";
        Debug.Log($"{conveyorItem} transfered from {this} to {nextConveyorName}");
        if (nextConveyor != null)
        {
            AddItem(conveyorItem);
        }
        else
        {
            Destroy(conveyorItem.gameObject);
        }
        Items.Remove(conveyorItem);
    }

    private void AddItem(ConveyorItem conveyorItem)
    {
        conveyorItem.moveProgress = 0;
        nextConveyor.Items.Add(conveyorItem);
    }

    public void ConnectTo(Conveyor conveyor)
    {
        nextConveyor = conveyor;
        conveyor.prevConveyor = this;

        Debug.Log($"{this} connected to: {nextConveyor}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(inputPoint.position, 0.4f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(outputPoint.position, 0.4f);
    }

}
