using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridManger : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int length = 20;
    [SerializeField] private float cellSize = 2f;

    private Dictionary<Vector2Int, GameObject> occupiedCells = new ();
    private Vector3 originPosition;

    private void Start()
    {
        originPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    public bool IsInsideGrid(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width
            && gridPosition.y >= 0 && gridPosition.y < length; 
    }

    private void DrawGrid()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x <= width; x++)
        {
            Vector3 startPos = originPosition + new Vector3(x * cellSize, 0, 0);
            Vector3 endPos = originPosition + new Vector3(x * cellSize, 0, length * cellSize);
            Gizmos.DrawLine(startPos, endPos);
        }
        for (int y = 0; y <= length; y++)
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

    public Vector3 GridToWorld(Vector2Int gridPosition)
        => originPosition 
        + new Vector3(gridPosition.x * cellSize + cellSize / 2f, 
                        0, 
                        gridPosition.y * cellSize + cellSize / 2f);

    public bool IsCellOccupied(Vector2Int position)
        => occupiedCells.ContainsKey(position);

    public void OccupiedCell(Vector2Int position, GameObject building)
        => occupiedCells.Add(position, building); 
}
