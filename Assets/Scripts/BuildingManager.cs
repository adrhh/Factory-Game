using System;
using System.Net.Http;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private readonly Vector2Int[] directions =
    {
        Vector2Int.zero,
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    [SerializeField] private GridManger gridManger;
    [SerializeField] private BuildingData[] building;
    [SerializeField] private BuildingPreview preview;
    [SerializeField] private LayerMask buildingMask;
    public static bool IsBuildingMode;

    private bool buildingMode;
    private bool removeMode;
    private int currentBuildingIndex;
    private BuildingData CurrentBuilding => building[currentBuildingIndex];
    private PlacedBuilding currentHighlightedBuilding;

    private const float conveyorDistanceToConnect = 0.4f;
    private const float conveyorDistanceToConnectToBuilding = 3f;


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
            IsBuildingMode = buildingMode;
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
            GameObject placedObject = Instantiate(CurrentBuilding.prefab, preview.transform.position, preview.transform.rotation);
            
            PlacedBuilding placedBuilding = placedObject.GetComponent<PlacedBuilding>();
            placedBuilding.origin = preview.CurrentGridPosition;
            placedBuilding.size = preview.CurrentSizeDate;

            gridManger.OccupyArea(preview.CurrentGridPosition, preview.CurrentSizeDate, placedObject);

            Conveyor conveyor = placedBuilding.GetComponent<Conveyor>();
            if (conveyor != null)
                ConnectConveyors(conveyor);

            IConveyorOutput outputBuilding = placedBuilding.GetComponent<IConveyorOutput>();
            if (outputBuilding != null)
                ConnectOutputBuilding(outputBuilding);

            DrillRig drillRig = placedBuilding.GetComponent<DrillRig>();
            if (drillRig != null)
                AssignOreNode(drillRig);
            
            RefreshNearbyConnections(preview.CurrentGridPosition);
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

    private void ConnectConveyors(Conveyor conveyor)
    {
        Conveyor[] conveyors = FindObjectsByType<Conveyor>(FindObjectsSortMode.None);

        foreach (var other in conveyors)
        {
            if (other == conveyor)
                continue;
            
            float outputDistance = 
                Vector3.Distance(conveyor.OutputPoint.position, other.InputPoint.position);

            if (outputDistance < conveyorDistanceToConnect)
                conveyor.ConnectTo(other);

            float inputDistance = 
                Vector3.Distance(conveyor.InputPoint.position, other.OutputPoint.position);

            if (inputDistance < conveyorDistanceToConnect)
                other.ConnectTo(conveyor);

            Debug.Log($"inputDistance: {inputDistance}, outputDistance: {outputDistance}");
        }
    }

    private void ConnectOutputBuilding(IConveyorOutput building)
    {
        Vector2Int outputGridPosition = gridManger.WorldToGrid(building.OutputPoint.position);
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPosition = outputGridPosition + direction;
            GameObject neighborBuilding = gridManger.GetBuilding(neighborPosition);
            if (neighborBuilding == null)
                continue;

            Conveyor conveyor = neighborBuilding.GetComponent<Conveyor>();
            if (conveyor == null)
                continue;
            
            float distance = Vector3.Distance(
                building.OutputPoint.position,
                conveyor.InputPoint.position);
            
            if (distance < conveyorDistanceToConnectToBuilding)
            {
                building.SetOutputConveyor(conveyor);
                break;
            }
        }
    }

    private void RefreshNearbyConnections(Vector2Int centerPosition)
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int checkPosition = centerPosition + direction;
            GameObject building = gridManger.GetBuilding(checkPosition);

            if (building == null)
                continue;

            IConveyorOutput outputBuilding = building.GetComponent<IConveyorOutput>();
            if (outputBuilding == null)
                continue;

            ConnectOutputBuilding(outputBuilding);
        }
    }

    private void AssignOreNode(DrillRig drillRig)
    {
        Collider[] colliders = Physics.OverlapBox(
            drillRig.transform.position,
            Vector3.one,
            Quaternion.identity);
        
        foreach (Collider collider in colliders)
        {
            OreNode oreNode = collider.GetComponent<OreNode>();
            if (oreNode == null)
                continue;
            else
            {
                drillRig.SetNode(oreNode);
                Debug.Log("Node assigned!");
                return;
            }
        }
    }
}
