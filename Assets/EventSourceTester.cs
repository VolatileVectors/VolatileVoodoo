using UnityEngine;
using VolatileVoodoo.Events;
using Random = UnityEngine.Random;

public class EventSourceTester : MonoBehaviour
{
    public DoubleEventSource doubleEvent;
    public Vector2IntEventSource vector2IntTestEvent;

    private float timer;
    private float delay;

    private void Start()
    {
        delay = Random.Range(2f, 5f);
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer < delay)
            return;

        timer -= delay;
        delay = Random.Range(2f, 5f);

        doubleEvent.Raise(Random.Range(100f, 200f));
        vector2IntTestEvent.Raise(new Vector2Int(Random.Range(100, 201), Random.Range(100, 201)));
    }
}