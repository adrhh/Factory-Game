using UnityEngine;

public interface IConveyorOutput
{
    public Transform OutputPoint { get; }
    public void SetOutputConveyor(Conveyor conveyor);
}
