using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
    [SerializeField] float itemSpacing = 0.5f;
    [SerializeField] bool drawGizmos = false;

    public Transform InputPoint => inputPoint;
    public Transform OutputPoint => outputPoint;
    public Conveyor nextConveyor;
    public Conveyor prevConveyor;

    // TEST
    [SerializeField] ConveyorItem test;

    private void Update()
    {
        MoveItems();
        SpawnTestItem();
    }

    private void OnDestroy()
    {
        foreach (var item in Items)
            if (item != null)
                Destroy(item.gameObject);
    }

    private bool IsBlocked()
    {
        if (nextConveyor == null)
        {
            return true;
        }
        return !CanAcceptItem();
    }

    private bool CanAcceptItem()
    {
        if (Items.Count == 0)
            return true;
    
        ConveyorItem firstItem = Items[0];
        return firstItem.moveProgress > itemSpacing;
    }

    private void SpawnTestItem()
    {
        if (Input.GetKeyDown(KeyCode.T) && test != null)
        {
            ConveyorItem item = Instantiate(test, InputPoint.position, Quaternion.identity);
            item.moveProgress = 0;
            Items.Add(item);
        }
    }
    private void MoveItems()
    {
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            ConveyorItem item = Items[i];
            float targetProgress = 1 - itemSpacing * i;
            if (i == 0 && IsBlocked())
            {
                targetProgress = 0.95f;
            }

            item.moveProgress += moveSpeed * Time.deltaTime;
            item.moveProgress = Mathf.Min(targetProgress, item.moveProgress);
            item.transform.position = GetItemPosition(item.moveProgress);
            if (item.moveProgress >= 1)
            {
                TransferItem(item);
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
        if (drawGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(inputPoint.position, 0.4f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(outputPoint.position, 0.4f);
        }
    }

}
