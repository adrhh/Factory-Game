using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    public ItemData ItemData => itemData;
    public float moveProgress;
    public Conveyor currentConveyor;
}
