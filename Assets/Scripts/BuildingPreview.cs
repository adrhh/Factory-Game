using Unity.VisualScripting;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private GridManger gridManger;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    private bool canPlace;
    public bool CanPlace => canPlace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector2Int gridPosition = gridManger.WorldToGrid(hit.point);
            Vector3 snappedPosition = gridManger.GridToWorld(gridPosition);

            if (gridManger.IsInsideGrid(gridPosition))
            {
                transform.position = snappedPosition;
                canPlace = !gridManger.IsCellOccupied(gridPosition);
            }
        }
    }
}
