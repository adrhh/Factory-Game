using UnityEngine;

public class PlacedBuilding : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material highlightMaterial;

    Renderer[] renderers;
    public Vector2Int origin;
    public Vector2Int size;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetHighlight(bool value)
    {
        Material targetMaterial = value ? highlightMaterial : defaultMaterial;
        foreach (var renderer in renderers)
        {
            renderer.material = targetMaterial;
        }
    }
}
