using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] Transform inputPoint;
    [SerializeField] Transform outputPoint;

    public Transform InputPoint => inputPoint;
    public Transform OutputPoint => outputPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(inputPoint.position, 0.4f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(outputPoint.position, 0.4f);
    }

}
