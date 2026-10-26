using System.Runtime.InteropServices;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private GridManger gridManger;
    [SerializeField] private Renderer[] currentRenderers;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform topOutline;
    [SerializeField] private Transform backOutline;
    [SerializeField] private Transform rightOutline;
    [SerializeField] private Transform leftOutline;
    [SerializeField] private Transform visualHolder;
    private bool canPlace;
    public bool CanPlace => canPlace;

    private BuildingData currentBuilding;
    private GameObject currentVisual;
    private Vector2Int currentGridPosition;
    public Vector2Int CurrentGridPosition => currentGridPosition;
    public Vector2Int CurrentSizeDate => CurrentSize;
    private int rotation = 0;
    private Vector2Int CurrentSize
        => rotation % 180 == 0 
        ? currentBuilding.size 
        : new Vector2Int(currentBuilding.size.y, currentBuilding.size.y);

    // Update is called once per frame
    void Update()
    {
        RotatePreview();
        FollowMouse();
        UpdateOutline();
        UpdateVisual();
    }

    public void SelectBuilding(BuildingData building)
    {
        currentBuilding = building;

        if (currentBuilding != null)
        {
            Destroy(currentVisual);
        }

        currentVisual = Instantiate(building.previewPrefab, visualHolder);
        currentRenderers = GetComponentsInChildren<Renderer>();
        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

    }

    private void UpdateVisual()
    {
        if (currentRenderers == null)
            return;
        
        Material targetMaterial = canPlace ? validMaterial : invalidMaterial;

        foreach (var renderer in currentRenderers)
            renderer.material = targetMaterial;
    }
        

    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, groundMask))
        {
            currentGridPosition = gridManger.WorldToGrid(hit.point);
            currentGridPosition = gridManger.ClampToGrind(currentGridPosition, CurrentSize);
            Vector3 snappedPosition = gridManger.GridToWorld(currentGridPosition, CurrentSize);
            
            transform.position = snappedPosition;
            canPlace = gridManger.CanPlaceBuilding(currentGridPosition, CurrentSize);
        }
    }

    private void RotatePreview()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation += 90;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    private void UpdateOutline()
    {
        float width = CurrentSize.x * 2;
        float height = CurrentSize.y * 2;
        float thickness = 0.1f;

        topOutline.localScale = new Vector3(width, thickness, thickness);
        backOutline.localScale = new Vector3(width, thickness, thickness);

        rightOutline.localScale = new Vector3(thickness, thickness, height);
        leftOutline.localScale = new Vector3(thickness, thickness, height);

        topOutline.localPosition = new Vector3(0, 0.2f, height / 2);
        backOutline.localPosition = new Vector3(0, 0.2f, - height / 2);

        rightOutline.localPosition = new Vector3(-width / 2, 0.2f, 0);
        leftOutline.localPosition = new Vector3(width / 2, 0.2f, 0);
    }
}
