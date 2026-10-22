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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (gridManger == null)
        // {
        //     var manger = GameObject.FindWithTag("Grid");
        //     gridManger = manger.GetComponent<GridManger>();
        // }
    }

    // Update is called once per frame
    void Update()
    {
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
            currentGridPosition = gridManger.ClampToGrind(currentGridPosition, currentBuilding.size);
            Vector3 snappedPosition = gridManger.GridToWorld(currentGridPosition, currentBuilding.size);
            
            transform.position = snappedPosition;
            canPlace = gridManger.CanPlaceBuilding(currentGridPosition, currentBuilding.size);
        }
    }
}
