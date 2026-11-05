using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GridManger gridManger;
    [SerializeField] private BuildingData[] building;
    [SerializeField] private BuildingPreview preview;
    [SerializeField] private LayerMask buildingMask;

    private bool buildingMode;
    private bool removeMode;
    private int currentBuildingIndex;
    private BuildingData CurrentBuilding => building[currentBuildingIndex];
    private PlacedBuilding currentHighlightedBuilding;

    private void Start()
    {
        buildingMode = false;
        removeMode = false;
        preview.gameObject.SetActive(buildingMode);
    }
    void Update()
    {
        ToggleBuildMode();
        ToggleRemovedMode();
        if (removeMode)
        {
            RemoveBuilding();
        }
        else if (buildingMode)
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
            if (removeMode && buildingMode)
                removeMode = false;
            preview.gameObject.SetActive(buildingMode);
        }
    }
    private void ToggleRemovedMode()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            removeMode = !removeMode;
            if (removeMode && buildingMode)
            {
                buildingMode = false;
                preview.gameObject.SetActive(buildingMode);
            }
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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentBuildingIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentBuildingIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentBuildingIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentBuildingIndex = 7;
        }

        preview.SelectBuilding(CurrentBuilding);
    }

    private void PlaceBuilding()
    {
        if (preview.CanPlace && Input.GetMouseButtonDown(0)) // 0 = left, 1 = right click 
        {
            GameObject placeObject = Instantiate(CurrentBuilding.prefab, preview.transform.position, preview.transform.rotation);
            
            PlacedBuilding placeBuilding = placeObject.GetComponent<PlacedBuilding>();
            placeBuilding.origin = preview.CurrentGridPosition;
            placeBuilding.size = preview.CurrentSizeDate;

            gridManger.OccupyArea(preview.CurrentGridPosition, preview.CurrentSizeDate, placeObject);
        }
    }

    private void RemoveBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, buildingMask))
        {
            PlacedBuilding placeBuilding = hit.collider.GetComponentInParent<PlacedBuilding>();

            if (placeBuilding == null)
            {
                UpdateRemoveHighlight(null);
                return;
            }

            UpdateRemoveHighlight(placeBuilding);
            
            if (Input.GetMouseButton(0))  // 0 = left, 1 = right click
            {
                gridManger.ClearArea(placeBuilding.origin, placeBuilding.size);
                UpdateRemoveHighlight(null);
                Destroy(placeBuilding.gameObject);
            }
        }
        else
        {
            UpdateRemoveHighlight(null);
        }
    }

    private void UpdateRemoveHighlight(PlacedBuilding placedBuilding)
    {
        currentHighlightedBuilding?.SetHighlight(false);
        currentHighlightedBuilding = placedBuilding;
        currentHighlightedBuilding?.SetHighlight(true);
    }
}
