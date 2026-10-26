using System;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GridManger gridManger;
    [SerializeField] private BuildingData[] building;
    [SerializeField] private BuildingPreview preview;

    private bool buildingMode;
    private int currentBuildingIndex;
    private BuildingData CurrentBuilding => building[currentBuildingIndex];
    private void Start()
    {
        buildingMode = false;
        preview.gameObject.SetActive(buildingMode);
    }
    void Update()
    {
        ToggleBuildMode();
        if (buildingMode)
        {
            SelectBuilding();
            PlaceBuilding();
        }
    }

    private void ToggleBuildMode()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildingMode = !buildingMode;
            preview.gameObject.SetActive(buildingMode);
        }
    }

    private void SelectBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBuildingIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBuildingIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBuildingIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBuildingIndex = 3;
        }

        preview.SelectBuilding(CurrentBuilding);
    }

    private void PlaceBuilding()
    {
        if (preview.CanPlace && Input.GetMouseButtonDown(0)) // 0 = left, 1 = right click 
        {
            GameObject placeObject = Instantiate(CurrentBuilding.prefab, preview.transform.position, preview.transform.rotation);
            gridManger.OccupyArea(preview.CurrentGridPosition, preview.CurrentSizeDate, placeObject);
        }
    }
}
