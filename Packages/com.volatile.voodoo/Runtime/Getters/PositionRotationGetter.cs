using UnityEngine;
using VolatileVoodoo.Runtime.Events;

namespace VolatileVoodoo.Runtime.Getters
{
    public class PositionRotationGetter : MonoBehaviour
    {
        public PositionRotationEventSource positionRotationEvent;

        private Vector3 lastPosition;
        private Quaternion lastRotation;

        private void Start()
        {
            var thisTransform = transform;
            lastPosition = thisTransform.position;
            lastRotation = thisTransform.rotation;
            positionRotationEvent.Raise(lastPosition, lastRotation);
        }

        private void Update()
        {
            var thisTransform = transform;
            if (thisTransform.position == lastPosition && thisTransform.rotation == lastRotation)
                return;

            lastPosition = thisTransform.position;
            lastRotation = thisTransform.rotation;
            positionRotationEvent.Raise(lastPosition, lastRotation);
        }
    }
}