using System.Collections.Generic;
using UnityEngine;
using VolatileVoodoo.Values;

public class ValueTester : MonoBehaviour
{
    public List<FloatValue> values;

    private float delay;

    private void Update()
    {
        delay += Time.unscaledDeltaTime;

        if (delay >= 2f) {
            delay -= 2f;
            foreach (var value in values) {
                value.Value = Random.value * 10f;
            }
        }
    }
}