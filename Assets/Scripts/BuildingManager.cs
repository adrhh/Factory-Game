using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GridManger gridManger;
    [SerializeField] private BuildingData currentBuilding;
    [SerializeField] private BuildingPreview preview;
    
    void Update()
    {
        PlaceBuilding();
    }

    private void PlaceBuilding()
    {
        if (preview.CanPlace && Input.GetMouseButtonDown(0)) // 0 = left, 1 = right click 
        {
            Vector3 placementPosition = preview.transform.position;
            GameObject placeObject = Instantiate(currentBuilding.prefab, placementPosition, Quaternion.identity);
            Vector2Int gridPosition = gridManger.WorldToGrid(placementPosition);
            gridManger.OccupiedCell(gridPosition, placeObject);

        }
    }
}
