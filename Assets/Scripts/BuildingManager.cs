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
            GameObject placeObject = Instantiate(currentBuilding.prefab, preview.transform.position, Quaternion.identity);
            gridManger.OccupyArea(preview.CurrentGridPosition, currentBuilding.size, placeObject);
        }
    }
}
