using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private BuildingData currentBuilding;
    [SerializeField] private GridManger gridManger;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private LayerMask groundMask;
    private bool canPlace;
    public bool CanPlace => canPlace;


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
        UpdateVisual();
    }

    private void UpdateVisual() 
        => mr.material = canPlace ? validMaterial : invalidMaterial;

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
}
