using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridManger : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private Transform gridVisual;

    private Dictionary<Vector2Int, GameObject> occupiedCells = new ();
    private Vector3 originPosition => transform.position;
    [Space]
    [SerializeField] private bool ShouldDrawGizmos = false;

    private void Start()
    {
        SetupGridVisual();
    }
    
    private void SetupGridVisual()
    {
        float worldWidth = width * cellSize;
        float worldHeight = height * cellSize;

        gridVisual.localScale = new Vector3(worldWidth / 10, 1, worldHeight / 10);
        gridVisual.position = originPosition + new Vector3(worldWidth / 2, 0.1f, worldHeight / 2);

        Renderer renderer = gridVisual.GetComponent<Renderer>();
        renderer.material.mainTextureScale = new Vector2(width, height);
    }   
    private void OnDrawGizmos()
    {
        if (ShouldDrawGizmos)
            DrawGrid();
    }
    private void DrawGrid()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x <= width; x++)
        {
            Vector3 startPos = originPosition + new Vector3(x * cellSize, 0, 0);
            Vector3 endPos = originPosition + new Vector3(x * cellSize, 0, height * cellSize);
            Gizmos.DrawLine(startPos, endPos);
        }
        for (int y = 0; y <= height; y++)
        {
            Vector3 startPos = originPosition + new Vector3(0, 0, y * cellSize);
            Vector3 endPos = originPosition + new Vector3(width * cellSize, 0, y * cellSize);
            Gizmos.DrawLine(startPos, endPos);
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - originPosition;
        int x = Mathf.FloorToInt(localPosition.x / cellSize);
        int z = Mathf.FloorToInt(localPosition.z / cellSize);

        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPosition, Vector2Int size)
    {
        Vector3 worldPosition = originPosition;

        worldPosition.x += gridPosition.x * cellSize;
        worldPosition.z += gridPosition.y * cellSize;

        worldPosition.x += (size.x * cellSize) / 2f;
        worldPosition.z += (size.y * cellSize) / 2f;
        
        return worldPosition;
    }

    public Vector2Int ClampToGrind(Vector2Int gridPosition, Vector2Int size)
    {
        int x = Mathf.Clamp(gridPosition.x, 0, width - size.x);
        int y = Mathf.Clamp(gridPosition.y, 0, height - size.y);
        return new Vector2Int(x, y);
    }

    public void OccupyArea(Vector2Int origin, Vector2Int size, GameObject building)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int gridPosition = origin + new Vector2Int(x, y);
                occupiedCells.Add(gridPosition, building);
            }
        }
    }

    public bool CanPlaceBuilding(Vector2Int origin, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int gridPosition = origin + new Vector2Int(x, y);
                if (occupiedCells.ContainsKey(gridPosition))
                    return false;
            }
        }
        return true;
    }

    // public bool IsInsideGrid(Vector2Int gridPosition)
    // {
    //     return gridPosition.x >= 0 && gridPosition.x < width
    //         && gridPosition.y >= 0 && gridPosition.y < length; 
    // }

    // public bool IsCellOccupied(Vector2Int position)
    //     => occupiedCells.ContainsKey(position);

    // public void OccupiedCell(Vector2Int position, GameObject building)
    //     => occupiedCells.Add(position, building); 
}
