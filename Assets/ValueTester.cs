using UnityEngine;
using VolatileVoodoo.Values;

public class ValueTester : MonoBehaviour
{
    public FloatValue floatValue;
    public IntValue intValue;

    private float delay;

    private void Update()
    {
        delay += Time.unscaledDeltaTime;

        if (delay >= 2f) {
            delay -= 2f;

            floatValue.Value = Random.value * 10f;
            intValue.Value = Random.Range(0, 11);
        }
    }
}