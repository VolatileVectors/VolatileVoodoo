using UnityEngine;
using VolatileVoodoo.Values;

public class ValueSetter : MonoBehaviour
{
    public DoubleValue doubleValue;
    public Vector2IntValue vectorValue;

    public void SetDouble(double newValue)
    {
        doubleValue.Value = newValue;
    }

    public void SetVector(Vector2Int newValue)
    {
        vectorValue.Value = newValue;
    }
}