using UnityEngine;
using VolatileVoodoo.Events;
using Random = UnityEngine.Random;

public class EventSourceTester : MonoBehaviour
{
    public FloatEventSource FloatTestEvent;
    public IntEventSource IntTestEvent;

    private float timer;
    private float delay;

    private void Start()
    {
        delay = Random.Range(2f, 5f);
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (!(timer >= delay)) 
            return;
        
        timer -= delay;
        delay = Random.Range(2f, 5f);
            
        FloatTestEvent.Raise(Random.Range(100f, 200f));
        IntTestEvent.Raise(Random.Range(100, 201));
    }
}