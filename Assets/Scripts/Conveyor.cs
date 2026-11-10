using System;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public enum ConveyorType
    {
        Straight,
        Corner
    }
    [SerializeField] Transform inputPoint;
    [SerializeField] Transform outputPoint;
    public List<ConveyorItem> Items = new ();
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] ConveyorType conveyorType;

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
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            ConveyorItem conveyorItem = Items[i];
            conveyorItem.moveProgress += moveSpeed * Time.deltaTime;
            conveyorItem.moveProgress = Mathf.Clamp01(conveyorItem.moveProgress);
            conveyorItem.transform.position = GetItemPosition(conveyorItem.moveProgress);
            if (conveyorItem.moveProgress >= 1)
            {
                TransferItem(conveyorItem);
            }
        }
    }

    private Vector3 GetItemPosition(float progress)
    {   
        if (conveyorType == ConveyorType.Straight)
        {
            return GetItemStraightPosition(progress);
        }

        return GetItemCornerPosition(progress);
    }

    private Vector3 GetItemStraightPosition(float progress)
    {
        return Vector3.Lerp(
            InputPoint.position,
            OutputPoint.position,
            progress);
    }

    private Vector3 GetItemCornerPosition(float progress)
    {   
        Vector3 center = transform.position;
        Vector3 startPosition = (inputPoint.position - center).normalized;
        Vector3 endPosition = (outputPoint.position - center).normalized;
        Vector3 direction = Vector3.Slerp(startPosition, endPosition, progress);

        float radius = Vector3.Distance(center, inputPoint.position);
        return direction * radius + center;
    }

    private void TransferItem(ConveyorItem conveyorItem)
    {
        string nextConveyorName = nextConveyor != null
            ? nextConveyor.name
            : "null";
        Debug.Log($"{conveyorItem} transfered from {this} to {nextConveyorName}");
        if (nextConveyor != null)
        {
            nextConveyor.AddItem(conveyorItem);
        }
        else
        {
            Destroy(conveyorItem.gameObject);
        }
        Items.Remove(conveyorItem);
    }

    private void AddItem(ConveyorItem conveyorItem)
    {
        conveyorItem.moveProgress = 0f;
        conveyorItem.transform.position = InputPoint.position;
        Items.Add(conveyorItem);
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
