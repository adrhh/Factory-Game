using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] Transform inputPoint;
    [SerializeField] Transform outputPoint;

    public Transform InputPoint => inputPoint;
    public Transform OutputPoint => outputPoint;
    public Conveyor nextConveyor;
    public Conveyor prevConveyor;

    public void ConnectTo(Conveyor conveyor)
    {
        nextConveyor = conveyor;
        conveyor.prevConveyor = this;

        Debug.Log($"{this} connected to: {nextConveyor}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(inputPoint.position, 0.4f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(outputPoint.position, 0.4f);
    }

}
