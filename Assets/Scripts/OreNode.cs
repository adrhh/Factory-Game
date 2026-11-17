using UnityEngine;

public class OreNode : MonoBehaviour
{
    [SerializeField] ConveyorItem orePrefab;
    public ConveyorItem OrePrefab => orePrefab;
}
